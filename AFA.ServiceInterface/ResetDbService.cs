using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFA.ServiceModel;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;

namespace AFA.ServiceInterface
{
    [Route("/reset-db", "GET,POST")]
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