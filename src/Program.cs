using System;

    string accessToken = await Alza.AuthenticateAsync();

    Console.WriteLine("Which boxes are available? ");
    string boxes = await Alza.GetBoxesAsync(accessToken);
    Console.WriteLine(boxes);

    Console.WriteLine("Which reservations were created? ");
    string reservations = await Alza.GetReservaionAsync(accessToken);
    Console.WriteLine(reservations);

    Console.ReadKey();