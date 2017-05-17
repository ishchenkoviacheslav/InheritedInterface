using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
namespace InheritedInterface
{
    interface IBase
    {
        void BaseFunc();

    }
    interface IHeir : IBase
    {
        void HeirFunc();
    }
    class MyClass : IBase
    {
        public void BaseFunc()
        {
            Console.WriteLine("hello base");
        }

        public void HeirFunc()
        {
            Console.WriteLine("hello heir");
        }
    }

    abstract class MyClass2
    {
        abstract public void fu();
    }
    class MyClass3 : MyClass2
    {
        public override void fu()
        {
            Console.WriteLine("abstract method overrided");
        }
    }

    public class MyMutexException : Exception
    {
        public MyMutexException() { }
        public MyMutexException(string message) : base(message) { }
        public MyMutexException(string message, Exception inner) : base(message, inner) { }
        protected MyMutexException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    class MyMutex
    {
        Mutex mutex = null;
        bool Locked = false;
        Action act;
        public async Task Lock()
        {
        await Task.Run(() =>
            {
                mutex = new Mutex();
                mutex.WaitOne();
                //act = () => { mutex.ReleaseMutex(); };
                Locked = true;
                Thread.CurrentThread.Name = "MyThread";
            });
            //returnedTask = new Task(() =>
            //{

            //    thread = new Thread(new ThreadStart(() =>
            //    {
            //        mutex.WaitOne();
            //    }));
            //    thread.Name = "myThread";
            //    thread.Start();
            //});
            //returnedTask.Start();
            //await returnedTask;
        }
        public void Release()
        {
            if (Locked)
            {
                act();
                Locked = false;
            }
            else
            {
                Locked = false;
                throw new MyMutexException("Only one of tasks, returned by Lock(), must become completed");
            }
        }
    }
    class Program
    {
        static MyMutex mutexObj = new MyMutex();
        static int x = 0;
        static void Main(string[] args)
        {

            //MyClass2 cl2 = new MyClass3();
            //cl2.fu();
            //Console.WriteLine(cl2.GetType().ToString());
            //IBase bs = new MyClass();
            //if(bs is IHeir)
            //{
            //    ((IHeir)bs).HeirFunc();
            //}
            //********************************************************
            //  Task task1 = new Task(() =>
            //  {
            //      Console.WriteLine("Id of Task: {0}", Task.CurrentId);
            //  });

            //  // задача продолжения
            //  Task task2 = task1.ContinueWith(Display);

            //  task1.Start();

            //  // ждем окончания второй задачи
            ////  task2.Wait();
            //  Console.WriteLine("Executing Task from method Main");
            //  Console.ReadLine();
            //*******************************************************

            //   Task task1 = new Task(() => {
            //       Console.WriteLine("Id of Task(I): {0}", Task.CurrentId);
            //   });


            //   // задача продолжения
            //   Task task2 = task1.ContinueWith(Display);

            //   Task task3 = task1.ContinueWith((Task t) =>
            //   {
            //       Console.WriteLine("Id of Task(II): {0}", Task.CurrentId);
            //   });
            ////   task3.Start(); InvalidOperationException
            //   Task task4 = task2.ContinueWith((Task t) =>
            //   {
            //       Console.WriteLine("Id of Task(III): {0}", Task.CurrentId);
            //   });


            //   task1.Start();
            //   task4.Wait();
            //****************************************************************
            for (int i = 0; i < 5; i++)
            {
                Thread myThread = new Thread(Count);
                myThread.Name = "Thread " + i.ToString();
                myThread.Start();
            }

            Console.ReadLine();
        }
        //static void Display(Task t)
        //{
        //    Console.WriteLine("Id of Task(Display): {0}", Task.CurrentId);
        //    Console.WriteLine("Id of Task befor: {0}", t.Id);
        //    Thread.Sleep(3000);
        //}
        public static async void Count()
        {
            await mutexObj.Lock();
            x = 1;
            for (int i = 1; i < 9; i++)
            {
                Console.WriteLine("{0}: {1}", Thread.CurrentThread.Name, x);
                x++;
                Thread.Sleep(100);
            }
            mutexObj.Release();
        }
    }
}
