using Blog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Blog.Services
{
    public class ChartViewModelGenerator
    {

        public ChartViewModel<string> Generate(IEnumerable<DateTime> datesOfCreation, DateTime from, 
            DateTime to, int daysInOneStep, string title)
        {
            ChartViewModel<string> model = new ChartViewModel<string>() {Title = title };          
            if(from >= to) 
            {
                throw new ArgumentException("Parametr From more To"); 
            }

            for (DateTime i = from; i < to; i = i.AddDays(daysInOneStep))
            {
                int commentsCount = datesOfCreation.Count(d => d > i && d < i.AddDays(daysInOneStep));
                model.XValues.Add(i.ToString("d") + "-" + i.AddDays(1).ToString("d"));
                model.YValues.Add(commentsCount);
            }
            return model;
        }
     
    }
}
