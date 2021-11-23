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

        public async Task<int> DeleteLabel(int labelId)
        {
            Labels deletelabel = await _labelRepository.GetLabel(labelId);
            if (deletelabel != null)
            {
                await _labelRepository.DeleteLabel(labelId);
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
            TodoItems todoItems = new TodoItems { ItemID = todoItem.ItemID, ItemName = todoItem.ItemName, Id = todoItem.ListId };
            TodoItems updatedItem = await _todoItemsRepository.UpdateTodoItem(todoItems);
            return updatedItem;
        }

        public async Task<int> DeleteToDoItem(int itemId)
        {
            TodoItems deletedItem = await _todoItemsRepository.GetTodoItem(itemId);
            if (deletedItem != null)
            {
                await _todoItemsRepository.DeleteTodoItem(itemId);
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
            TodoLists todoList = new TodoLists { TodoListName = todolist.TodoListName, Id = todolist.ListId };
            TodoLists updatedItem = await _todoListRepository.UpdateTodoList(todoList);
            return updatedItem;
        }

        public async Task<int> DeleteToDoList(int ListId)
        {
            TodoLists deletedItem = await _todoListRepository.GetTodoList(ListId);
            if (deletedItem != null)
            {
                await _todoListRepository.DeleteTodoList(ListId);
                return deletedItem.Id;
            }
            return -1;
        }

    }
}
