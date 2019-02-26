using Building.Interfaces;
using Building.Model.PartOfHouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Building.Model
{
    public class HouseProject
    {
        public HouseProject()
        {
            createProject();
            team.createTeam();
            createTasks();
        }

        private Team team = new Team();

        public List<IPart> parts = new List<IPart>();
        public List<ITask> tasks = new List<ITask>();
        public List<Payment> payments = new List<Payment>();
        private Random rnd = new Random();

        public void startBuilding()
        {
            ITask newTask = nonCompleateTask();
            while (newTask != null)
            {
                IWorker worker = team.getWorker();
                if (worker.position == Position.manager)
                {                    
                    getProcessInfo();
                    Thread.Sleep(1000);
                }
                else
                {                    
                    tasks[newTask.id].startDate = DateTime.Now;
                    tasks[newTask.id].endDate =
                        tasks[newTask.id].startDate
                                         .AddDays(rnd.Next(2, 30));
                    tasks[newTask.id].status = Status.comlite;
                    tasks[newTask.id].idWorker = worker.id;

                    double diffDay = (tasks[newTask.id].endDate - tasks[newTask.id].startDate).TotalDays;
                    TimeSpan ts = tasks[newTask.id].endDate - tasks[newTask.id].startDate;
                    Payment pay = new Payment() { worker = worker, totalSalary = worker.calcSalary(ts) };
                    payments.Add(pay);

                    Console.WriteLine("Работа - {0} над {1} началась {2}",
                        worker.fullName, tasks[newTask.id].part.name,
                        tasks[newTask.id].startDate);

                    for (int i = 0; i < diffDay; i++)
                    {
                        Console.Write(".");
                        Thread.Sleep(500);
                    }
                    Console.WriteLine("");
                    Console.WriteLine("Работы завершены: {0}", tasks[newTask.id].endDate);
                    //Console.WriteLine("Оплата: {0}", pay.totalSalary);
                    Console.WriteLine("");
                }

                newTask = nonCompleateTask();
            }

            Console.WriteLine("Строительство завершено!");
            Console.Write("Нажмите что-нибудь: ");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("-------------Ведомость по оплате труда рабочим----------\n");
            var paymentsGroup = from payment in payments
                                group payment by payment.worker;
            foreach (IGrouping<IWorker, Payment> i in paymentsGroup)
            {
                double x = 0;                
                foreach (var j in i)
                {
                    x += j.totalSalary;
                }
                Console.WriteLine("Оплата для {0} = {1}", i.Key.fullName, x);
            }
        }

        private ITask nonCompleateTask()
        {
            return tasks.FirstOrDefault(w => w.status == Status.create);
        }

        public void getProcessInfo()
        {
            int k = 0;
            Console.WriteLine("---------Промежуточный отчет о строительстве-----------\n");
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].status.Equals(Status.comlite))
                {
                    Console.WriteLine("Строительство {0} завершилось в {1}", tasks[i].part.name, tasks[i].endDate);
                    k++;
                }
            }
            Console.WriteLine("На данный момент завершено {0}% строительства\n", (k*100)/tasks.Count);
        }

        public void createProject()
        {
            IPart pBasment = new Basement()
            {
                name = "Basement",
                price = rnd.Next(),
                count = 1,
                order = 0
            };
            IPart pWalls = new Walls()
            {
                name = "Walls",
                price = rnd.Next(),
                count = 4,
                order = 1,
                color = Color.grey
            };
            IPart pDoors = new Doors()
            {
                name = "Doors",
                price = rnd.Next(),
                count = 2,
                order = 2,
                material = Material.dsp
            };
            IPart pWindows = new Windows()
            {
                name = "Windows",
                price = rnd.Next(),
                count = 4,
                order = 3
            };
            IPart pRoof = new Roof()
            {
                name = "Roof",
                price = rnd.Next(),
                count = 1,
                order = 4,
                roofTypes = RoofTypes.sloped
            };

            parts.Add(pBasment);
            parts.Add(pWalls);
            parts.Add(pDoors);
            parts.Add(pWindows);
            parts.Add(pRoof);
        }

        public void createTasks()
        {
            int k = 0;
            foreach (IPart part in parts.OrderBy(o => o.order))
            {
                for (int i = 0; i < part.count; i++)
                {
                    ITask task = new Task()
                    {
                        id = k++,
                        part = part
                    };
                    tasks.Add(task);
                }
            }
        }
    }
}
