using System;

string accessToken = await Alza.AuthenticateAsync();

    Console.WriteLine("Which boxes are available? ");
    string boxes = await Alza.GetBoxesAsync(accessToken);
    Console.WriteLine(boxes);

    Console.WriteLine("Which reservations are created? ");
    string reservations = await Alza.GetReservationsAsync(accessToken);
    Console.WriteLine(reservations);