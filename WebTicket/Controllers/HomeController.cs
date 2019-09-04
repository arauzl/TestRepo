using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using WebTicket.Models;

namespace WebTicket.Controllers
{
    public class HomeController : Controller
    {
        Timer _tm = null;
        AutoResetEvent _autoEvent = null;
        private List<ColaViewModel> lista = new List<ColaViewModel>();
        private List<ColaViewModel> lista2 = new List<ColaViewModel>();


        public IActionResult Index()
        {

            IndexViewModel model = new IndexViewModel();
            using (ticketDBContext db = new ticketDBContext())
            {
                lista = (from d in db.Colas
                         join t in db.Ticket
                         on d.TicketId equals t.Id
                         where d.Cola == 1
                         select new ColaViewModel
                         {
                             id = d.Id,
                             cola = d.Cola.Value,
                             ticket_id = d.TicketId.Value,
                             nombre = t.Nombre
                         }
                ).ToList();

                lista2 = (from d in db.Colas
                          join t in db.Ticket
                          on d.TicketId equals t.Id
                          where d.Cola == 2
                          select new ColaViewModel
                          {
                              id = d.Id,
                              cola = d.Cola.Value,
                              ticket_id = d.TicketId.Value,
                              nombre = t.Nombre
                          }
                ).ToList();

            }

            model.cola1 = lista;
            model.cola2 = lista2;
            return View(model);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult CrearTicket(IndexViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int cola = 1;
                    //Se calcula el tiempo de espera de cada cola.
                    int tCola1 = tiempoCola(1, 2);
                    int tCola2 = tiempoCola(2, 3);

                    if (tCola1 <= tCola2)
                    {
                        cola = 1;
                    }
                    else
                    {
                        cola = 2;
                    }

                    using (ticketDBContext db = new ticketDBContext())
                    {
                        var oTicket = new Ticket();
                        oTicket.Id = model.ticket_id;
                        oTicket.Nombre = model.nombre;
                        if (db.Ticket.Find(oTicket.Id) == null)
                        {
                            db.Ticket.Add(oTicket);
                            db.SaveChanges();
                        }




                        var oCola = new Colas();
                        oCola.Cola = cola;
                        oCola.TicketId = oTicket.Id;
                        db.Colas.Add(oCola);
                        db.SaveChanges();


                        lista = (from d in db.Colas
                                 join t in db.Ticket
                                 on d.TicketId equals t.Id
                                 where d.Cola == 1
                                 select new ColaViewModel
                                 {
                                     id = d.Id,
                                     cola = d.Cola.Value,
                                     ticket_id = d.TicketId.Value,
                                     nombre = t.Nombre
                                 }
                            ).ToList();

                        lista2 = (from d in db.Colas
                                  join t in db.Ticket
                                  on d.TicketId equals t.Id
                                  where d.Cola == 2
                                  select new ColaViewModel
                                  {
                                      id = d.Id,
                                      cola = d.Cola.Value,
                                      ticket_id = d.TicketId.Value,
                                      nombre = t.Nombre
                                  }
                            ).ToList();

                    }
                    model.cola1 = lista;
                    model.cola2 = lista2;

                    return PartialView("_ListaCola", model);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.InnerException.Message);
            }

            return null;
        }

        public int tiempoCola(int cola, int duracion)
        {
            int tiempo = 0;

            using (ticketDBContext db = new ticketDBContext())
            {
                lista = (from d in db.Colas
                         where d.Cola == cola
                         select new ColaViewModel
                         {
                             id = d.Id,
                             cola = d.Cola.Value,
                             ticket_id = d.TicketId.Value

                         }
                ).ToList();
            }

            tiempo = lista.Count() * duracion;

            return tiempo;
        }

        [HttpPost]
        public IActionResult BorrarTicket(IndexViewModel model)
        {

            using (ticketDBContext db = new ticketDBContext())
            {
                lista = (from d in db.Colas
                         where d.Cola == model.cola
                         select new ColaViewModel
                         {
                             id = d.Id,
                             cola = d.Cola.Value,
                             ticket_id = d.TicketId.Value

                         }
                ).ToList();
                if (lista.Count > 0)
                {
                    ColaViewModel primerTicket = lista.ElementAt(0);
                    Colas primeroCola = new Colas();

                    primeroCola.Id = primerTicket.id;
                    primeroCola.Cola = primerTicket.cola;
                    primeroCola.TicketId = primerTicket.ticket_id;

                    db.Colas.Remove(primeroCola);
                    db.SaveChanges();
                }

                lista = (from d in db.Colas
                         join t in db.Ticket
                         on d.TicketId equals t.Id
                         where d.Cola == 1
                         select new ColaViewModel
                         {
                             id = d.Id,
                             cola = d.Cola.Value,
                             ticket_id = d.TicketId.Value,
                             nombre = t.Nombre
                         }
                            ).ToList();

                lista2 = (from d in db.Colas
                          join t in db.Ticket
                          on d.TicketId equals t.Id
                          where d.Cola == 2
                          select new ColaViewModel
                          {
                              id = d.Id,
                              cola = d.Cola.Value,
                              ticket_id = d.TicketId.Value,
                              nombre = t.Nombre
                          }
                    ).ToList();

            }
            model.cola1 = lista;
            model.cola2 = lista2;


            return PartialView("_ListaCola", model);

        }

    }






}

