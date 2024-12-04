using System;

    SimpleLogger logger = new SimpleLogger($"log-{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
    logger.LogInfo("Start");

    string accessToken = await Alza.AuthenticateAsync(logger);

    Console.WriteLine("Which boxes are available? ");
    string boxes = await Alza.GetBoxesAsync(accessToken, logger);
    Console.WriteLine(boxes);

    Console.WriteLine("Which reservations are created? ");
    string reservations = await Alza.GetReservationsAsync(accessToken, logger);
    Console.WriteLine(reservations);