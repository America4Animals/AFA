using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFA.ServiceModel;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;

namespace AFA.ServiceInterface
{
    [Route("/resetdb", "GET,POST")]
    public class ResetDb { }

    public class ResetDbResponse
    {
    }

    public class ResetDbService : ServiceStack.ServiceInterface.Service
    {
        public object Any(ResetDb request)
        {
            Db.CreateTableIfNotExists<StateProvince>();
            Db.CreateTableIfNotExists<OrganizationCategory>();
            Db.CreateTableIfNotExists<Organization>();
            //Db.CreateTableIfNotExists<OrganizationOrganizationCategory>();
            Db.CreateTableIfNotExists<User>();
            Db.CreateTableIfNotExists<OrganizationAlly>();
            Db.CreateTableIfNotExists<OrganizationNews>();
            Db.CreateTableIfNotExists<OrganizationComment>();
            Db.CreateTableIfNotExists<EventCategory>();
            Db.CreateTableIfNotExists<Event>();
            Db.CreateTableIfNotExists<CrueltySpot>();
            Db.CreateTableIfNotExists<CrueltySpotCategory>();
            //Db.DropAndCreateTable<CrueltySpot>();
            //Db.DropAndCreateTable<CrueltySpotCategory>();

            if (Db.Select<StateProvince>().Count == 0)
            {
                var newYork = new StateProvince
                {
                    Abbreviation = "NY",
                    Name = "New York"
                };

                var california = new StateProvince
                {
                    Abbreviation = "CA",
                    Name = "California"
                };

                Db.Insert(newYork);
                Db.Insert(california);
            }

            if (Db.Select<CrueltySpotCategory>().Count == 0)
            {
                var crueltySpotCategories = new List<CrueltySpotCategory>
                                                {
                                                    new CrueltySpotCategory {Name = "Foie Gras"},
                                                    new CrueltySpotCategory {Name = "Shark Fin"},
                                                    new CrueltySpotCategory {Name = "Circuses & Rodeos"},
                                                    new CrueltySpotCategory {Name = "Races"},
                                                    new CrueltySpotCategory {Name = "Captive Performances"},
                                                    new CrueltySpotCategory {Name = "Puppy Mill Stores (formerly Pet Stores)"},
                                                    new CrueltySpotCategory {Name = "Fur Stores"},
                                                    new CrueltySpotCategory {Name = "Horse Carriages"},
                                                    new CrueltySpotCategory {Name = "Labs"},
                                                    new CrueltySpotCategory {Name = "Other"}
                                                };

                Db.InsertAll(crueltySpotCategories);
            }

            if (Db.Select<EventCategory>().Count == 0)
            {
                var eventCategories = new List<EventCategory>
                                          {
                                              new EventCategory {Name = "Demo"},
                                              new EventCategory {Name = "Dinner"},
                                              new EventCategory {Name = "Film Screening"},
                                              new EventCategory {Name = "Fundraiser"},
                                              new EventCategory {Name = "Info Session"},
                                              new EventCategory {Name = "Meeting"},
                                              new EventCategory {Name = "Outreach"},
                                              new EventCategory {Name = "Party"},
                                              new EventCategory {Name = "Potluck"},
                                              new EventCategory {Name = "Protest"},
                                              new EventCategory {Name = "Social"}
                                          };

                Db.InsertAll(eventCategories);
            }

            return new ResetDbResponse();
        }
    }
}