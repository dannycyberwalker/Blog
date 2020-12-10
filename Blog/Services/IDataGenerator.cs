
using System;
using System.Collections.Generic;

namespace Blog.Services
{
    public interface IDataGenerator
    {
        /// <summary>
        /// Return JSON array as string.
        /// </summary>
        /// <param name="DateOfCreation">Date of creation record.</param>
        /// <param name="StepOfDays">How many days in one cycle iteration.</param>
        /// <returns></returns>
        public string Generate(IEnumerable<DateTime> DatesOfCreation, 
            DateTime From, DateTime To, int StepOfDays);
    }
}
