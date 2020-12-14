using Blog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Blog.Services
{
    public class ChartViewModelGenerator
    {

        public ChartViewModel<string> Generate(IEnumerable<DateTime> DatesOfCreation, DateTime From, 
            DateTime To, int StepOfDays, string Title)
        {
            ChartViewModel<string> model = new ChartViewModel<string>() {Title = Title };          
            if(From >= To) 
            {
                throw new ArgumentException("Parametr From more To"); 
            }

            for (DateTime i = From; i < To; i = i.AddDays(StepOfDays))
            {
                int commentsCount = DatesOfCreation.Where(d => d > i && d < i.AddDays(StepOfDays)).Count();
                model.XValues.Add(i.ToString("d") + "-" + i.AddDays(1).ToString("d"));
                model.YValues.Add(commentsCount);
            }
            return model;
        }
     
    }
}
