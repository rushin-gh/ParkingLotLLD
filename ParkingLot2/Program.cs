using System.Collections.Concurrent;
using ParkingLot2.Classes;
using ParkingLot2.Classes.PaymentStrategy;
using ParkingLot2.Classes.RateCalculationStratergy;
using ParkingLot2.Enums;

var parkingManager = ParkingManager.Instance;

// Concurrency demo: 50 cars arrive at the same time for 10 car spots.
var tickets = new ConcurrentBag<Ticket>();
var rejected = 0;

Parallel.For(0, 50, i =>
{
    var car = new Vehicle { VehicleType = VehicleType.Car, RegNumber = $"CAR{i:D3}" };
    try
    {
        tickets.Add(parkingManager.Park(car));
    }
    catch (InvalidOperationException)
    {
        Interlocked.Increment(ref rejected);
    }
});

var carSpots = parkingManager.Spots.Count(s => s.vehicleType == VehicleType.Car);
var distinctSpots = tickets.Select(t => t.Spot.Id).Distinct().Count();
var distinctIds = tickets.Select(t => t.Tid).Distinct().Count();

Console.WriteLine($"Car spots: {carSpots}, parked: {tickets.Count}, rejected: {rejected}");
Console.WriteLine($"Double-booked spots: {tickets.Count - distinctSpots}, duplicate ticket ids: {tickets.Count - distinctIds}");

// Same ticket scanned at two exit gates at once: only one exit should succeed.
var ticket = tickets.First();
var exits = 0;
Parallel.For(0, 10, _ =>
{
    if (parkingManager.Unpark(new HourlyStratergy(), new CardPay(), ticket))
        Interlocked.Increment(ref exits);
});

Console.WriteLine($"Successful exits for one ticket: {exits}, spot now: {ticket.Spot.Status}, ticket now: {ticket.Status}");
