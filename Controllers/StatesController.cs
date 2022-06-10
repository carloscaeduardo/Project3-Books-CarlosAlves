using Project3_Books_CarlosAlves.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project3_Books_CarlosAlves.Controllers
{
    public class StatesController : Controller
    {
        // GET: States
        /// <summary>
        /// Get the All States view with the sorting and searching features
        /// based on the table columns
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <param name="isDesc"></param>
        /// <returns>returns the AllStates view</returns>
        public ActionResult AllStates(string id, int sortBy = 0, bool isDesc = false)
        {
            var context = new BooksEntities();
            List<State> states = context.States.ToList();

            switch (sortBy)
            {
           
                case 1:
                    {
                        if (isDesc)
                        {
                            states = context.States.OrderByDescending(s => s.StateName).ToList();
                        }
                        else
                        {
                            states = context.States.OrderBy(s => s.StateName).ToList();
                        }

                        break;
                    }
            
                case 0:
                default:
                    {
                        if (isDesc)
                        {
                            states = context.States.OrderByDescending(s => s.StateCode).ToList();
                        }
                        else
                        {
                            states = context.States.OrderBy(s => s.StateCode).ToList();
                        }


                        break;
                    }
                   

            }


            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                states = states.Where(s =>
                                       s.StateCode.ToLower().Contains(id) ||
                                       s.StateName.ToLower().Contains(id)).ToList();


            }

            return View(states);
        }


        /// <summary>
        /// Http GET request to retrieve the infomation to the upsert state view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return stateToSave view</returns>
        [HttpGet]
        public ActionResult UpsertState(string id)
        {
            BooksEntities context = new BooksEntities();
            var stateToSave = context.States.Where(s => s.StateCode == id).FirstOrDefault();
            if (stateToSave == null)
            {
                stateToSave = new State();
            }
            return View(stateToSave);
        }
        /// <summary>
        /// Http POST request to update or insert a new State to the table
        /// </summary>
        /// <param name="newState"></param>
        /// <returns>redirects to the All States view</returns>
        [HttpPost]
        public ActionResult UpsertState(State newState)
        {
            BooksEntities context = new BooksEntities();

            try
            {
                if (context.States.Where(s => s.StateCode == newState.StateCode).Count() > 0)
                {
                    var stateToSave = context.States.Where(s => s.StateCode == newState.StateCode).FirstOrDefault();

                    stateToSave.StateName = newState.StateName;
    

                }
                else
                {
                    context.States.Add(newState);
                }
                context.SaveChanges();




            }
            catch (System.Exception)
            {
                throw;
                // log the exception in the error log, and send an automatic email to IT Support
                //return RedirectToAction("Error");
            }
            return RedirectToAction("AllStates");
        }

        /// <summary>
        /// Http GET delete request. Deletes a row on the state view based on the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>redirect to All states</returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            BooksEntities context = new BooksEntities();
            string stateCode = id;
          
            try
            {
                State state = context.States.Where(s => s.StateCode == stateCode).FirstOrDefault();
                context.States.Remove(state);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
   
            return RedirectToAction("AllStates");
        }
    }
}