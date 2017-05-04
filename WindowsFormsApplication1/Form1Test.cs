using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    [TestFixture]
    class Form1Test
    {
        Form1 form1;
        Event testEvent;

        [SetUp]
        protected void SetUp()
        {
            form1 = new Form1();
            testEvent = new Event();
        }

        //Form1.load() was trying to open the first event even though
        //no events were loaded from the database. This test failed on that
        //once fixed, the test works
        [Test]
        public void LoadTest_DisplayFirstEvent()
        {
            //need to ensure that no events exist in the database
            DateTime thisDay = DateTime.Today;  //get today's date
            string myString = thisDay.ToShortDateString();
            ArrayList events = testEvent.getEventList(myString);

            if(events.Count > 0)
            {
                foreach(Event e in events) {
                    e.deleteEvent();
                }
            }

            //had to move code that was in Form1.Load() to Form1.LoadEvents() so that it was easier to test
            form1.LoadEvents();
        }
    }
}
