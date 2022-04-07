CREATE VIEW AllReservations
AS
SELECT p.FirstName,p.LastName,v.Brand,v.Model,v.Plate,r.Start,r.Finish
FROM Reservations r INNER JOIN People p
ON r.IdPerson=p.Id
INNER JOIN Vehicles v
ON r.IdVehicle=v.Id