using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class GoogleChartDataGenerator : IDataGenerator
    {

        public string Generate(IEnumerable<DateTime> DatesOfCreation, DateTime From, 
            DateTime To, int StepOfDays)
        {
            if(From >= To) 
            {
                throw new ArgumentException("Parametr From more To"); 
            }

            List<List<int>> jsonData = new List<List<int>>();
            int j = 0;
            for (DateTime i = From; i < To; i = i.AddDays(StepOfDays))
            {
                int commentsCount = DatesOfCreation.Where(d => d > i && d < i.AddDays(StepOfDays)).Count();
                jsonData.Add(new List<int> { j, commentsCount });
                j++;
            }
            return JsonSerializer.Serialize(jsonData);
        }
    }
}
