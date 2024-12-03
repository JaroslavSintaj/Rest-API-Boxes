using System;

    string accessToken = await Alza.AuthenticateAsync();
/*
    Console.WriteLine("Which boxes are available? ");
    string boxes = await Alza.GetBoxesAsync(accessToken);
    Console.WriteLine(boxes);

    Console.WriteLine("Which reservations were created? ");
    string reservations = await Alza.GetReservationAsync(accessToken);
    Console.WriteLine(reservations);
*/
    
    string reservationID = "id_f80938f5-d089-4b37-bf75-1c8217681971";
    //vygenerovat id 
    //pouzit predpripravenou metodu pro data
    //vytvorit reservation
    // kontrola vytvoreni
    // precteni JSON a jeho parsovani
    //Console.WriteLine(await Alza.GetReservationByIDAsync(accessToken,reservationID));
    Console.ReadKey();