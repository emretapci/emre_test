using System;
using System.Collections.Generic;
using System.Linq;

namespace Viagogo
{
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class Solution
    {
        static void Main(string[] args)
        {
            var events = new List<Event>{
                new Event{ Name = "Phantom of the Opera", City = "New York"},
                new Event{ Name = "Metallica", City = "Los Angeles"},
                new Event{ Name = "Metallica", City = "New York"},
                new Event{ Name = "Metallica", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "New York"},
                new Event{ Name = "LadyGaGa", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "Chicago"},
                new Event{ Name = "LadyGaGa", City = "San Francisco"},
                new Event{ Name = "LadyGaGa", City = "Washington"}
                };

            Question1(events);
            Question2(events);
            Question3_4(events);

            var eventsWithPrices = new List<Event>{
                new Event{ Name = "Phantom of the Opera", City = "New York"},
                new Event{ Name = "Metallica", City = "Los Angeles"},
                new Event{ Name = "Metallica", City = "New York"},
                new Event{ Name = "Metallica", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "New York"},
                new Event{ Name = "LadyGaGa", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "Chicago"},
                new Event{ Name = "LadyGaGa", City = "San Francisco"},
                new Event{ Name = "LadyGaGa", City = "Washington"}
                };


            Question5(eventsWithPrices);
        }

        static void Question1(List<Event> events)
        {
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };

            //1.1. To get the list of events, we query the list of events with the City constraint.
            var query = from result in events
                        where result.City.Equals(customer.City)
                        select result;

            //Then add all events into the email
            foreach (var item in query)
            {
                //1.2
                AddToEmail(customer, item);
            }

            //1.3. The expected output when we call this method with customer.Name = "John Smith" depends on the customer's city.

            //1.4. We can improve this code by keeping all events in a dictionary with the keys of event cities.
            //So that, when we want to query events based on city, we can do it in O(1) time.
        }

        static void Question2(List<Event> events)
        {
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };

            //We expand the event items with distances to the customer's city.
            var eventsWithDistances = (from event_ in events
                                       select (Event: event_, Distance: GetDistance(customer.City, event_.City)))
                                      .ToList();

            //Get the 5 least distance events
            var eventsWith5LeastDistance = new List<Event>();
            while (eventsWithDistances.Count > 0 && eventsWith5LeastDistance.Count < 5)
            {
                //Find the minimum distance event in the list
                var minIndex = 0;
                for (int i = 0; i < eventsWithDistances.Count; i++)
                {
                    if (eventsWithDistances[i].Distance < eventsWithDistances[minIndex].Distance)
                    {
                        minIndex = i;
                    }
                }
                var minDistanceEvent = eventsWithDistances[minIndex];

                //Add it to the eventsWith5LeastDistance list
                eventsWith5LeastDistance.Add(eventsWithDistances[minIndex].Event);

                //Remove it from the events list
                eventsWithDistances.RemoveAt(minIndex);
            }

            //Add all events into the email
            foreach (var event_ in eventsWith5LeastDistance)
            {
                AddToEmail(customer, event_);
            }
        }

        static void Question3_4(List<Event> events)
        {
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };

            //Keep a lookup table for city distances.
            var cities = events.Select(e => e.City).Distinct().ToArray();
            var cityIndexLookupTable = cities.Select((city, index) => (City: city, Index: index)).ToDictionary(pair => pair.City, pair => pair.Index);

            int[,] distances = new int[cities.Length, cities.Length];

            for (int i = 0; i < cities.Length; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (i == j)
                    {
                        distances[j, i] = 0;
                    }
                    else
                    {
                        var distance = int.MaxValue;
                        try
                        {
                            distance = GetDistance(cities[i], cities[j]);
                        }
                        catch (Exception)
                        {
                            Console.Out.WriteLine($"Cannot determine distance from {cities[i]} to {cities[j]}. Infinity assumed.");
                        }

                        distances[j, i] = GetDistance(cities[i], cities[j]);
                        distances[i, j] = distances[j, i];
                    }
                }
            }

            var eventsWithDistances = new List<(Event Event, int Distance)>();
            foreach (var event_ in events)
            {
                eventsWithDistances.Add((event_, distances[cityIndexLookupTable[customer.City], cityIndexLookupTable[event_.City]]));
            }

            //Get the 5 least distance events
            var eventsWith5LeastDistance = new List<Event>();
            while (eventsWithDistances.Count > 0 && eventsWith5LeastDistance.Count < 5)
            {
                //Find the minimum distance event in the list
                var minDistanceEventPair = eventsWithDistances.MinBy(pair => pair.Distance);

                //Add it to the eventsWith5LeastDistance list
                eventsWith5LeastDistance.Add(minDistanceEventPair.Event);

                //Remove it from the events list
                eventsWithDistances.Remove(minDistanceEventPair);
            }

            //Add all events into the email
            foreach (var event_ in eventsWith5LeastDistance)
            {
                AddToEmail(customer, event_);
            }
        }

        static void Question5(List<Event> events)
        {
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };

            //Keep a lookup table for city distances.
            var cities = events.Select(e => e.City).Distinct().ToArray();
            var cityIndexLookupTable = cities.Select((city, index) => (City: city, Index: index)).ToDictionary(pair => pair.City, pair => pair.Index);

            int[,] distances = new int[cities.Length, cities.Length];

            for (int i = 0; i < cities.Length; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (i == j)
                    {
                        distances[j, i] = 0;
                    }
                    else
                    {
                        var distance = int.MaxValue;
                        try
                        {
                            distance = GetDistance(cities[i], cities[j]);
                        }
                        catch (Exception)
                        {
                            Console.Out.WriteLine($"Cannot determine distance from {cities[i]} to {cities[j]}. Infinity assumed.");
                        }

                        distances[j, i] = GetDistance(cities[i], cities[j]);
                        distances[i, j] = distances[j, i];
                    }
                }
            }

            var eventsWithDistancesAndPrices = new List<(Event Event, int Distance, int Price)>();
            foreach (var event_ in events)
            {
                eventsWithDistancesAndPrices.Add((event_, distances[cityIndexLookupTable[customer.City], cityIndexLookupTable[customer.City]], GetPrice(event_)));
            }

            //Get the 5 least distance events
            var eventsWith5LeastDistanceAndPrice = new List<Event>();
            while (eventsWithDistancesAndPrices.Count > 0 && eventsWith5LeastDistanceAndPrice.Count < 5)
            {
                //Find the minimum distance event in the list
                //If multiple events have the same minimum distance, get the one with less price
                var minDistanceEventPair = eventsWithDistancesAndPrices.MinBy(pair => pair, Comparer<(Event Event, int Distance, int Price)>.Create((p1, p2) =>
                {
                    if (p1.Distance != p2.Distance)
                    {
                        return p1.Distance.CompareTo(p2.Distance);
                    }
                    else
                    {
                        return p1.Price.CompareTo(p2.Price);
                    }
                }));

                //Add it to the eventsWith5LeastDistance list
                eventsWith5LeastDistanceAndPrice.Add(minDistanceEventPair.Event);

                //Remove it from the events list
                eventsWithDistancesAndPrices.Remove(minDistanceEventPair);
            }

            //Add all events into the email
            foreach (var event_ in eventsWith5LeastDistanceAndPrice)
            {
                AddToEmail(customer, event_);
            }
        }

        // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
                + (distance > 0 ? $" ({distance} miles away)" : "")
                + (price.HasValue ? $" for ${price}" : ""));
        }

        static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }

        static int GetDistance(string fromCity, string toCity)
        {
            return AlphebiticalDistance(fromCity, toCity);
        }

        private static int AlphebiticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i < Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
}
