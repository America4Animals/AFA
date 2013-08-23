using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android.Library.Helpers
{
    public static class StateNamesAndAbbreviations
    {
        static StateNamesAndAbbreviations()
        {
            StateNameLookup = new Dictionary<string, string>()
                                  {
                                        {"AL","Alabama"},
                                        {"AK","Alaska"},
                                        {"AZ","Arizona"},
                                        {"AR","Arkansas"},
                                        {"CA","California"},
                                        {"CO","Colorado"},
                                        {"CT","Connecticut"},
                                        {"DE","Delaware"},
                                        {"DC","District of Columbia"},
                                        {"FL","Florida"},
                                        {"GA","Georgia"},
                                        {"HI","Hawaii"},
                                        {"ID","Idaho"},
                                        {"IL","Illinois"},
                                        {"IN","Indiana"},
                                        {"IA","Iowa"},
                                        {"KS","Kansas"},
                                        {"KY","Kentucky"},
                                        {"LA","Louisiana"},
                                        {"ME","Maine"},
                                        {"MT","Montana"},
                                        {"NE","Nebraska"},
                                        {"NV","Nevada"},
                                        {"NH","New Hampshire"},
                                        {"NJ","New Jersey"},
                                        {"NM","New Mexico"},
                                        {"NY","New York"},
                                        {"NC","North Carolina"},
                                        {"ND","North Dakota"},
                                        {"OH","Ohio"},
                                        {"OK","Oklahoma"},
                                        {"OR","Oregon"},
                                        {"MD","Maryland"},
                                        {"MA","Massachusetts"},
                                        {"MI","Michigan"},
                                        {"MN","Minnesota"},
                                        {"MS","Mississippi"},
                                        {"MO","Missouri"},
                                        {"PA","Pennsylvania"},
                                        {"RI","Rhode Island"},
                                        {"SC","South Carolina"},
                                        {"SD","South Dakota"},
                                        {"TN","Tennessee"},
                                        {"TX","Texas"},
                                        {"UT","Utah"},
                                        {"VT","Vermont"},
                                        {"VA","Virginia"},
                                        {"WA","Washington"},
                                        {"WV","West Virginia"},
                                        {"WI","Wisconsin"},
                                        {"WY","Wyoming"}
                                  };


            StateAbbreviationLookup = new Dictionary<string, string>
                                          {
                                              {"Alabama","AL"},
                                            {"Alaska","AK"},
                                            {"Arizona","AZ"},
                                            {"Arkansas","AR"},
                                            {"California","CA"},
                                            {"Colorado","CO"},
                                            {"Connecticut","CT"},
                                            {"Delaware","DE"},
                                            {"District of Columbia","DC"},
                                            {"Florida","FL"},
                                            {"Georgia","GA"},
                                            {"Hawaii","HI"},
                                            {"Idaho","ID"},
                                            {"Illinois","IL"},
                                            {"Indiana","IN"},
                                            {"Iowa","IA"},
                                            {"Kansas","KS"},
                                            {"Kentucky","KY"},
                                            {"Louisiana","LA"},
                                            {"Maine","ME"},
                                            {"Montana","MT"},
                                            {"Nebraska","NE"},
                                            {"Nevada","NV"},
                                            {"New Hampshire","NH"},
                                            {"New Jersey","NJ"},
                                            {"New Mexico","NM"},
                                            {"New York","NY"},
                                            {"North Carolina","NC"},
                                            {"North Dakota","ND"},
                                            {"Ohio","OH"},
                                            {"Oklahoma","OK"},
                                            {"Oregon","OR"},
                                            {"Maryland","MD"},
                                            {"Massachusetts","MA"},
                                            {"Michigan","MI"},
                                            {"Minnesota","MN"},
                                            {"Mississippi","MS"},
                                            {"Missouri","MO"},
                                            {"Pennsylvania","PA"},
                                            {"Rhode Island","RI"},
                                            {"South Carolina","SC"},
                                            {"South Dakota","SD"},
                                            {"Tennessee","TN"},
                                            {"Texas","TX"},
                                            {"Utah","UT"},
                                            {"Vermont","VT"},
                                            {"Virginia","VA"},
                                            {"Washington","WA"},
                                            {"West Virginia","WV"},
                                            {"Wisconsin","WI"},
                                            {"Wyoming","WY"}
                                          };
        }

        public static Dictionary<string, string> StateNameLookup;
        public static Dictionary<string, string> StateAbbreviationLookup;
    }
}