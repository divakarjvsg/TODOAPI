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
        private readonly ITodoListsRepository _todoListRepository;
        private readonly ITodoItemsRepository _todoItemsRepository;
        private readonly ILabelsRepository _labelRepository;
        public TodoMutation([Service] ITodoListsRepository todoListRepository, [Service] ITodoItemsRepository todoItemsRepository, [Service] ILabelsRepository labelRepository)
        {
            _todoListRepository = todoListRepository;
            _todoItemsRepository = todoItemsRepository;
            _labelRepository = labelRepository;
        }

        public async Task<Labels> AddLabel(LabelModel label)
        {
            Labels addlabel = new Labels { LabelName = label.LabelName };
            Labels addedItem = await _labelRepository.AddLabels(addlabel);
            return addedItem;
        }

        public async Task<int> DeleteLabel(int id)
        {
            Labels deletelabel = await _labelRepository.GetLabel(id);
            if (deletelabel != null)
            {
                await _labelRepository.DeleteLabel(id);
                return deletelabel.LabelId;
            }
            return -1;
        }

        public async Task<TodoItems> AddToDoItem(TodoItemModel todoItem)
        {
            TodoItems todoItems = new TodoItems { ItemName = todoItem.ItemName, Id = todoItem.ListId };
            TodoItems addedItem = await _todoItemsRepository.AddTodoItem(todoItems);
            return addedItem;
        }

        public async Task<TodoItems> UpdateToDoItem(UpdateTodoItemModel todoItem)
        {
            TodoItems todoItems = new TodoItems { ItemID = todoItem.ItemID, ItemName = todoItem.ItemName, Id = todoItem.Id };
            TodoItems updatedItem = await _todoItemsRepository.UpdateTodoItem(todoItems);
            return updatedItem;
        }

        public async Task<int> DeleteToDoItem(int id)
        {
            TodoItems deletedItem = await _todoItemsRepository.GetTodoItem(id);
            if (deletedItem != null)
            {
                await _todoItemsRepository.DeleteTodoItem(id);
                return deletedItem.ItemID;
            }
            return -1;
        }


        public async Task<TodoLists> AddToDoList(TodoListModel todoList)
        {
            TodoLists todolist = new TodoLists { TodoListName = todoList.TodoListName };
            TodoLists addedItem = await _todoListRepository.AddTodoList(todolist);
            return addedItem;
        }

        public async Task<TodoLists> UpdateToDoList(UpdateTodoListModel todolist)
        {
            TodoLists todoList = new TodoLists { TodoListName = todolist.TodoListName, Id = todolist.Id };
            TodoLists updatedItem = await _todoListRepository.UpdateTodoList(todoList);
            return updatedItem;
        }

        public async Task<int> DeleteToDoList(int id)
        {
            TodoLists deletedItem = await _todoListRepository.GetTodoList(id);
            if (deletedItem != null)
            {
                await _todoListRepository.DeleteTodoList(id);
                return deletedItem.Id;
            }
            return -1;
        }

    }
}
