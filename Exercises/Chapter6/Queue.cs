using System.Collections.Generic;

namespace Homework
{
    /// <summary>
    /// FIFO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Queue<T> where T : IResettable
    {
        private List<T> items;

        public Queue()
        {
            items = new List<T>();
        }

        public void Enqueue(T item)
        {
            //Add to the end of the list
            items.Add(item);
        }

        public T Dequeue()
        {
            if (items.Count > 0)
            {
                var item = items[0];
                items.RemoveAt(0);
                return item;
            }

            throw new QueueEmptyException("No items are in the queue");
        }

        public void Reset()
        {
            foreach (var item in items)
                item.Reset();
        }
    }
}