using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    [TestFixture]
    class Tests
    {
        Form1 form1;
        Event testEvent;

        [SetUp]
        protected void SetUp()
        {
            form1 = new Form1();
            testEvent = new Event();
        }

        private void deleteTodaysEvents()
        {
            //need to ensure that no events exist in the database for this date
            DateTime thisDay = DateTime.Today;  //get today's date
            string myString = thisDay.ToShortDateString();
            ArrayList events = testEvent.getEventList(myString);

            if (events.Count > 0)
            {
                foreach (Event e in events)
                {
                    e.deleteEvent();
                }
            }
        }


        //Adding an event to the calendar
        [Test]
        public void LoadTest_DisplayFirstEvent_NoEvents()
        {
            //ensure that no events exist for the day
            deleteTodaysEvents();

            //had to move code that was in Form1.Load() to Form1.LoadEvents() so that it was easier to test
            form1.LoadEvents();

            DateTime thisDay = DateTime.Today;  //get today's date
            string myString = thisDay.ToShortDateString();
            Assert.AreEqual(new Event().getEventList(myString).Count, 0);
        }

        [Test]
        public void LoadTest_DisplayFirstEvent_OneOrMoreEvents()
        {
            //ensure that an event on this date exists in the database
            DateTime thisDay = DateTime.Today;
            string dateString = thisDay.ToShortDateString();

            Event newEvent = new Event("title", dateString, 0, 1, "event stuff");
            newEvent.saveNewEvent();

            form1.LoadEvents();
        }

        [Test]
        public void saveNewEvent_saveSameTime()
        {
            deleteTodaysEvents();

            Event newEvent = new Event("title", DateTime.Today.ToShortDateString(), 0, 0, "event stuff");
            bool saveEventResult = newEvent.saveNewEvent();

            Assert.False(saveEventResult); //saveNewEvent should not work
        }

        [Test]
        public void saveNewEvent_saveNormal()
        {
            deleteTodaysEvents();

            Event newEvent = new Event("title", DateTime.Today.ToShortDateString(), 1, 2, "event stuff");
            bool saveEventResult = newEvent.saveNewEvent();

            Assert.True(saveEventResult);
        }

        [Test]
        public void saveNewEvent_blankTitle()
        {
            deleteTodaysEvents();

            Event newEvent = new Event("", DateTime.Today.ToShortDateString(), 1, 2, "event");
            bool saveEventResult = newEvent.saveNewEvent();

            Assert.False(saveEventResult);
        }

        [Test]
        public void saveNewEvent_whiteSpaceTitle()
        {
            deleteTodaysEvents();

            Event newEvent = new Event("     ", DateTime.Today.ToShortDateString(), 1, 2, "event");
            bool saveEventResult = newEvent.saveNewEvent();

            Assert.False(saveEventResult);
        }


        //Deleting an Event From the Calendar
        [Test]
        public void deleteEvent_blankTitle()
        {
            //create an event with a blank title
            //call the Form1.button8_click method that confirms the deletion of the event
            //let the deletion happen
            //try to look up the event
            //ensure that nothing is returned

            deleteTodaysEvents();

            Event newEvent = new Event("", DateTime.Today.ToShortDateString(), 1, 2, "event stuff");
            newEvent.saveNewEvent();

            ArrayList events = newEvent.getEventList(DateTime.Today.ToShortDateString());
            foreach (Event e in events)
            {
                form1.eventList.Add(e);
            }
            
            form1.textBox1.Text = newEvent.getTitle();
            form1.textBox2.Text = DateTime.Today.ToShortDateString();
            form1.comboBox1.SelectedItem = 1;
            form1.comboBox2.SelectedItem = 2;
            form1.richTextBox1.Text = "event stuff";
            form1.button8_Click(null, new EventArgs());
            
            Assert.AreEqual(0, form1.eventList.Count);
        }


        //Viewing an Event in the Calendar
        [Test]
        public void saveNewEvent_invalidTime()
        {
            deleteTodaysEvents();

            Event newEvent = new Event("", DateTime.Today.ToShortDateString(), 0, -1, "event");
            bool saveEventResult = newEvent.saveNewEvent();

            Assert.False(saveEventResult);
        }

        [Test]
        public void saveNewEvent_titleTooLarge()
        {
            deleteTodaysEvents();

            Event newEvent = new Event("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbcccccc",
                DateTime.Today.ToShortDateString(), 0, 1, "event");
            bool saveEventResult = newEvent.saveNewEvent();

            Assert.False(saveEventResult);
        }

        [Test]
        public void saveNewEvent_titleFiftyChar()
        {
            deleteTodaysEvents();

            Event newEvent = new Event("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb",
                DateTime.Today.ToShortDateString(), 0, 1, "event");
            bool saveEventResult = newEvent.saveNewEvent();

            Assert.False(saveEventResult);
        }

        [Test]
        public void saveNewEvent_descTooLarge()
        {
            deleteTodaysEvents();

            Event newEvent = new Event("title",
                DateTime.Today.ToShortDateString(), 0, 1,
                "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbcccccccc");
            bool saveEventResult = newEvent.saveNewEvent();

            Assert.False(saveEventResult);
        }

        [Test]
        public void saveNewEvent_desc200Char()
        {
            deleteTodaysEvents();

            Event newEvent = new Event("title",
                DateTime.Today.ToShortDateString(), 0, 1,
                "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbcccccccc");
            bool saveEventResult = newEvent.saveNewEvent();

            Assert.False(saveEventResult);
        }


    }
}
