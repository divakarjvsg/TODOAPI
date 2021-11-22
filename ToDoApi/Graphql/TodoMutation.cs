using HotChocolate;
using System.Threading.Tasks;
using TodoAPI.Models;
using TodoAPI.Models.UpdateModels;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoAPI.Graphql
{
    public class TodoMutation
    {
        private readonly ITodoListRepository todoListRepository;
        private readonly ITodoItemsRepository todoItemsRepository;
        private readonly ILabelRepository labelRepository;
        public TodoMutation([Service] ITodoListRepository todoListRepository, [Service] ITodoItemsRepository todoItemsRepository, [Service] ILabelRepository labelRepository)
        {
            this.todoListRepository = todoListRepository;
            this.todoItemsRepository = todoItemsRepository;
            this.labelRepository = labelRepository;
        }

        public async Task<Labels> AddLabel(LabelModel label)
        {
            Labels addlabel = new Labels { LabelName = label.LabelName };
            Labels addedItem = await labelRepository.AddLabels(addlabel);
            return addedItem;
        }

        public async Task<int> DeleteLabel(int id)
        {
            Labels deletelabel = await labelRepository.GetLabel(id);
            if (deletelabel != null)
            {
                await labelRepository.DeleteLabel(id);
            }
            return deletelabel.LabelId;
        }

        public async Task<TodoItems> AddToDoItem(TodoItemModel todoItem)
        {
            TodoItems todoItems = new TodoItems { ItemName = todoItem.ItemName, Id = todoItem.ListId };
            TodoItems addedItem = await todoItemsRepository.AddTodoItem(todoItems);
            return addedItem;
        }

        public async Task<TodoItems> UpdateToDoItem(UpdateTodoItemModel todoItem)
        {
            TodoItems todoItems = new TodoItems {ItemID=todoItem.ItemID,ItemName=todoItem.ItemName,Id=todoItem.Id };
            TodoItems updatedItem = await todoItemsRepository.UpdateTodoItem(todoItems);
            return updatedItem;
        }

        public async Task<int> DeleteToDoItem(int id)
        {
            TodoItems deletedItem = await todoItemsRepository.GetTodoItem(id);
            if (deletedItem != null)
            {
                await todoItemsRepository.DeleteTodoItem(id);
            }
            return deletedItem.ItemID;
        }


        public async Task<TodoLists> AddToDoList(TodoListModel todoList)
        {
            TodoLists todolist = new TodoLists { TodoListName = todoList.TodoListName};
            TodoLists addedItem = await todoListRepository.AddTodoList(todolist);
            return addedItem;
        }

        public async Task<TodoLists> UpdateToDoList(UpdateTodoListModel todolist)
        {
            TodoLists todoList = new TodoLists { TodoListName = todolist.TodoListName, Id = todolist.Id };
            TodoLists updatedItem = await todoListRepository.UpdateTodoList(todoList);
            return updatedItem;
        }

        public async Task<int> DeleteToDoList(int id)
        {
            TodoLists deletedItem = await todoListRepository.GetTodoList(id);
            if (deletedItem != null)
            {
                await todoListRepository.DeleteTodoList(id);
            }
            return deletedItem.Id;
        }

    }
}
