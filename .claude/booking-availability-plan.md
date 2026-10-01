# Booking availability plan

Goal: a customer can't book a time that's already taken, and the Time step shows taken
slots as unavailable.

## Where things stand

- **The API accepts double bookings.** `Create` in
  `MudBarber.ApiService/Endpoints/BookingEndpoints.cs` only checks that the barber and
  service exist, then saves.
- **The time slots are hardcoded.** `TimeStep.razor` builds them from `OpenAt`/`CloseAt`
  in 30-minute steps. Nothing is fetched.
- **ConfirmStep already shows API errors.** `BookingApiClient.CreateBookingAsync` reads a
  400 `ValidationProblem` into `CreateBookingResult.Errors`, and `ConfirmStep` displays
  them.
- **All steps stay mounted.** `Stepper.razor` renders every step and hides the inactive
  ones, so a step's `OnInitializedAsync` runs once per page visit, not on every
  navigation.

## Slice 1: the API rejects overlapping bookings (~20 min)

This slice is the real guard. A stale UI can't create a double booking if the server
refuses it.

1. In `BookingEndpoints.Create`, once `service` is known:
   - `newStart = request.Start`
   - `newEnd = request.Start.AddMinutes(service.EstimatedMinutes)`
2. Load that barber's bookings near the requested day, then check for overlap in memory.
   Two bookings overlap when `existing.Start < newEnd && existingEnd > newStart`, where
   `existingEnd = existing.Start.AddMinutes(existing.EstimatedMinutes)`.
   - Loading the day first avoids depending on EF translating `AddMinutes` on a column.
     If you want the whole check in SQL, confirm the provider translates it.
3. On overlap: `errors[nameof(request.Start)] = ["This time was just booked."]`, then
   return the existing `ValidationProblem`.
4. Test: book a slot, book the same slot again → the second attempt shows the error in
   ConfirmStep.

**Known gap:** two requests that arrive at the same moment can both pass the check.
Closing that needs a DB-level guarantee (for example a Postgres exclusion constraint on
barber + time range). Leave it for later; note it in `deferred-cleanups`.

## Slice 2: availability endpoint (~30 min)

1. Add `GET /barbers/{id:guid}/bookings?date=yyyy-MM-dd` to `BookingEndpoints` (or
   `BarberEndpoints`).
2. Return the taken ranges for that barber and day, e.g. a list of `{ Start, End }`. Don't
   return full `BookingDto`s, so customer names aren't exposed.
3. **Day boundaries:** ConfirmStep builds `Start` using the server's local offset. Use the
   same offset when turning `date` into a `[dayStart, dayEnd)` range, or the slots near
   midnight will come out wrong.
4. Add a matching method to `BookingApiClient` (e.g. `GetTakenSlotsAsync(barberId, date)`).

## Slice 3: TimeStep uses it (~30 min)

1. Inject `BookingApiClient` into `TimeStep.razor`.
2. In `SelectDate`, once `Model.Date` is set, fetch the taken ranges for `Model.Barber` and
   that date, and store them in a field (e.g. `_taken`).
3. Add `IsTaken(TimeOnly time)`: does `[time, time + service minutes)` overlap any range in
   `_taken`? Use `Model.Service.EstimatedMinutes` so a 60-minute service can't start
   30 minutes before another booking.
4. In `TimeCardClass`, add a `taken` class and ignore clicks on those slots in
   `SelectTime`. Style it in `TimeStep.razor.css`.
5. Add loading and failed states, matching the ones in `ServiceStep`.

## Forcing a refetch after a conflict

Once fetching happens in `SelectDate`, forcing a refetch is cheap:

- When ConfirmStep gets the "just booked" error, set `Model.Date = null` and
  `Model.Time = null`, then send the user back to the Time step. Picking a date again
  fetches fresh slots.
- ConfirmStep can't change the stepper's active step yet. Give it an `EventCallback`
  (e.g. `OnConflict`) that `Reserve.razor` hands to `Stepper`, the same way `OnStepEdit`
  bubbles up from `StepperStep`.

**If clearing the date feels clunky:** add `int AvailabilityVersion` to `BookingModel`.
ConfirmStep increments it on a conflict. TimeStep remembers the `(barberId, date, version)`
of its last load and refetches in `OnParametersSetAsync` when any of them change. The user
keeps their date, and the slots refresh by themselves.

## Edge case

Changing the barber or service after picking a time can leave `Model.Time` on a slot that
is taken for the new barber or too long for the new service. Slice 1 catches this at
submit. Clearing `Model.Time` when the barber or service changes would catch it earlier.
