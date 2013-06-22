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
            Db.CreateTableIfNotExists<CrueltySpotCategory>();
            Db.CreateTableIfNotExists<CrueltySpot>();
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

                var dc = new StateProvince
                {
                    Abbreviation = "DC",
                    Name = "District of Columbia"
                };

                var virginia = new StateProvince
                {
                    Abbreviation = "VA",
                    Name = "Virginia"
                };

                Db.Insert(newYork);
                Db.Insert(california);
                Db.Insert(dc);
                Db.Insert(virginia);
            }

            if (Db.Select<CrueltySpotCategory>().Count == 0)
            {
                var crueltySpotCategories = new List<CrueltySpotCategory>
                                                {
                                                    new CrueltySpotCategory
                                                        {
                                                            Name = "Foie Gras", 
                                                            IconName = "foiegras.png",
                                                            Description = "Foie gras (“fatty liver”) is made by jamming a tube down the throats of ducks or geese and force feeding them until their livers are 10 times the normal size. More than a dozen countries and the state of California have banned the production or sale of foie gras."
                                                        },

                                                    new CrueltySpotCategory
                                                        {
                                                            Name = "Shark Fin", 
                                                            IconName = "shark.png",
                                                            Description = "Shark fins are cut off the backs of live sharks, who are then thrown back into the ocean. Unable to swim without their fins, the sharks are eaten by predators or painfully succumb to suffocation. All so the fins can be served as a delicacy in East-Asian cuisine. Rising demand for shark fin has contributed to a staggering drop in the shark population over the past 50 years. Some species are near extinction. Shark fining has been banned in several countries."
                                                        },

                                                    new CrueltySpotCategory
                                                        {
                                                            Name = "Circuses & Rodeos", 
                                                            IconName = "rodeo.png",
                                                            Description = "Circus animals endure pain and abuse from a young age. They are trained to performed tricks by use of chains, bull hooks, and blowtorches. No government agency is required to monitor these training sessions. At rodeos, horses and calves are stung with electric prods to aggravate them before a performance. Straps are pulled tightly across their torsos, causing them to buck in pain throughout the show."
                                                        },

                                                    new CrueltySpotCategory
                                                        {
                                                            Name = "Races", 
                                                            IconName = "races.png",
                                                            Description = "Horses and greyhounds are bred and raised in tight and unsanitary conditions. They are made to endure grueling training and forced to race at high speeds, often in extreme heat. More than 700 horses per year die of heat and exhaustion during races and many horses and dogs suffer serious injuries, such as broken legs. When no longer able to race due to injury or old age, the animals are killed or sold into animal testing or food production."
                                                        },

                                                    new CrueltySpotCategory
                                                        {
                                                            Name = "Captive Performances", 
                                                            IconName = "performance.png",
                                                            Description = "Zoos, marine mammal parks, and animal display facilities hold animals captive in small enclosures and unfamiliar environments, often forcing them to perform for viewers. Many captive animals suffer from “zoochosis” – a psychosis caused by grim living conditions and the lack of the freedom and stimulation of their natural environments. Mood-altering drugs like Prozac are typically administered to lessen those symptoms that are visible to the public."
                                                        },

                                                    new CrueltySpotCategory
                                                        {
                                                            Name = "Puppy Mill Stores (formerly Pet Stores)", 
                                                            IconName = "petstores.png",
                                                            Description = "Puppies, kittens, and other “pure-bred” animals sold in pet stores are most often born and raised in mill-like breeding facilities known as puppy mills. Breeding dogs and cats are impregnated repeatedly and experience little human or animal interaction. Once they can no longer breed, they are killed or disposed of. By selling dogs and cats that were bred for money, pet stores deprive shelter animals of loving homes and contribute to over-population."
                                                        },

                                                    new CrueltySpotCategory
                                                        {
                                                            Name = "Fur Stores", 
                                                            IconName = "fur.png",
                                                            Description = "Factory fur farms produce much of the fur sold today. There, rabbits and minks are kept is small cages and denied the freedom to walk, nest, or mate. Anal or genital electrocution is a common method of slaughter. Wild animals such as bears and seals are trapped for their skins, as well. Steel jaw traps cut through skin and bone as the animals struggle to escape. Trapped and wounded, they often die of exposure or predator attacks before humans collect them for their fur."
                                                        },

                                                    new CrueltySpotCategory
                                                        {
                                                            Name = "Horse Carriages", 
                                                            IconName = "cariagges.png",
                                                            Description = "Carriage horses are forced to work long days pulling oversized carriages. Extreme weather conditions and the lack of laws protecting them lead to incapacitating leg pain, respiratory disease, heatstroke, and even death. Once seriously injured or too old to work, carriage horses are slaughtered for animal food or the overseas meat market. Horse carriages are banned in several U.S. cities."
                                                        },

                                                    new CrueltySpotCategory
                                                        {
                                                            Name = "Labs", 
                                                            IconName = "labs.png",
                                                            Description = "Over 100 million animals each year are used in laboratory studies for cosmetic, medical, and consumer products in the United States. These animals include dogs, rabbits, mice, and great apes. They are forced to endure burns, ingest toxic chemicals, and contract fatal diseases. Despite all this suffering, 92% of drugs successfully tested on animals are found ineffective in human trials. Alternative procedures to animal testing are readily available and used by many successful companies."
                                                        },

                                                    new CrueltySpotCategory
                                                        {
                                                            Name = "Other", 
                                                            IconName = "more.png"
                                                        }
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