
# Todo Api

Todo Api is to create and manage the todo lists with items. Can assign labels to lists and items.

Api uses local SQL database.



## API Reference

#### Get all TodoLists

```http
  GET /api/TodoLists
```

#### Get TodoLists item

```http
  GET /api/TodoLists/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **Required**. Id of item to fetch |

#### POST TodoLists item

```http
  POST /api/TodoLists
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `TodoLists`      | `object` | **Required**. item to insert |

#### DELETE TodoLists item

```http
  DELETE /api/TodoLists/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **Required**. Id of item to delete |

```http
  GET /api/TodoLists/SearchList/${search}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `search`      | `string` | **Required**. name of item to search |


```http
  POST /api/Labels/Assign/${SelectedGuid}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `SelectedGuid`      | `Guid` | **Required**. name of list/item to assign a label |
| `labelId`      | `json` | **Required**. Id of the label to assign selected Guid |


