import {
  Button,
  Card,
  Input,
  message,
  Popconfirm,
  Space,
  Table,
  Tag,
  Typography,
} from "antd";
import type { ColumnsType } from "antd/es/table";
import { useEffect, useMemo, useState } from "react";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import TicketCategoryFormModal from "../../features/ticketCategories/components/TicketCategoryFormModal";
import {
  selectIsTicketCategoriesLoading,
  selectIsTicketCategoryMutating,
  selectTicketCategories,
  selectTicketCategoryError,
} from "../../features/ticketCategories/ticketCategorySelectors";
import {
  createTicketCategoryThunk,
  deleteTicketCategoryThunk,
  getTicketCategoriesThunk,
  updateTicketCategoryThunk,
} from "../../features/ticketCategories/ticketCategoryThunks";
import type {
  CreateTicketCategoryRequest,
  TicketCategory,
} from "../../features/ticketCategories/ticketCategoryTypes";

const { Title, Text } = Typography;

export default function TicketCategoriesPage() {
  const dispatch = useAppDispatch();

  const categories = useAppSelector(selectTicketCategories);
  const isLoading = useAppSelector(selectIsTicketCategoriesLoading);
  const isMutating = useAppSelector(selectIsTicketCategoryMutating);
  const error = useAppSelector(selectTicketCategoryError);

  const [searchText, setSearchText] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<TicketCategory | null>(
    null,
  );

  useEffect(() => {
    dispatch(getTicketCategoriesThunk());
  }, [dispatch]);

  useEffect(() => {
    if (error) {
      message.error(error.message);
    }
  }, [error]);

  const filteredCategories = useMemo(() => {
    const keyword = searchText.trim().toLowerCase();

    if (!keyword) {
      return categories;
    }

    return categories.filter((category) => {
      const name = category.name.toLowerCase();
      const description = category.description?.toLowerCase() ?? "";

      return name.includes(keyword) || description.includes(keyword);
    });
  }, [categories, searchText]);

  const handleOpenCreateModal = () => {
    setEditingCategory(null);
    setIsModalOpen(true);
  };

  const handleOpenEditModal = (category: TicketCategory) => {
    setEditingCategory(category);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setEditingCategory(null);
    setIsModalOpen(false);
  };

  const handleSubmit = async (values: CreateTicketCategoryRequest) => {
    if (editingCategory) {
      const result = await dispatch(
        updateTicketCategoryThunk({
          id: editingCategory.id,
          request: values,
        }),
      );

      if (updateTicketCategoryThunk.fulfilled.match(result)) {
        message.success("Ticket category updated successfully.");
        handleCloseModal();
      }

      return;
    }

    const result = await dispatch(createTicketCategoryThunk(values));

    if (createTicketCategoryThunk.fulfilled.match(result)) {
      message.success("Ticket category created successfully.");
      handleCloseModal();
    }
  };

  const handleDelete = async (id: string) => {
    const result = await dispatch(deleteTicketCategoryThunk(id));

    if (deleteTicketCategoryThunk.fulfilled.match(result)) {
      message.success("Ticket category deleted successfully.");
    }
  };

  const columns: ColumnsType<TicketCategory> = [
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
      sorter: (a, b) => a.name.localeCompare(b.name),
      render: (value: string) => <Text strong>{value}</Text>,
    },
    {
      title: "Description",
      dataIndex: "description",
      key: "description",
      render: (value?: string | null) => value || "-",
    },
    {
      title: "Status",
      dataIndex: "isActive",
      key: "isActive",
      width: 120,
      render: (isActive?: boolean) => {
        if (isActive === undefined) {
          return "-";
        }

        return isActive ? (
          <Tag color="green">Active</Tag>
        ) : (
          <Tag color="red">Inactive</Tag>
        );
      },
    },
    {
      title: "Actions",
      key: "actions",
      width: 180,
      render: (_, record) => (
        <Space>
          <Button type="link" onClick={() => handleOpenEditModal(record)}>
            Edit
          </Button>

          <Popconfirm
            title="Delete category"
            description="Are you sure you want to delete this category?"
            okText="Delete"
            cancelText="Cancel"
            okButtonProps={{ danger: true }}
            onConfirm={() => handleDelete(record.id)}
          >
            <Button type="link" danger>
              Delete
            </Button>
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <>
      <Card>
        <Space direction="vertical" size="large" style={{ width: "100%" }}>
          <Space
            align="center"
            style={{
              width: "100%",
              justifyContent: "space-between",
            }}
          >
            <div>
              <Title level={3} style={{ marginBottom: 4 }}>
                Ticket Categories
              </Title>
              <Text type="secondary">
                Manage categories used when users create tickets.
              </Text>
            </div>

            <Button type="primary" onClick={handleOpenCreateModal}>
              Create Category
            </Button>
          </Space>

          <Input.Search
            allowClear
            placeholder="Search by name or description"
            value={searchText}
            onChange={(event) => setSearchText(event.target.value)}
            style={{ maxWidth: 360 }}
          />

          <Table
            rowKey="id"
            columns={columns}
            dataSource={filteredCategories}
            loading={isLoading}
            pagination={{
              pageSize: 8,
              showSizeChanger: false,
            }}
          />
        </Space>
      </Card>

      <TicketCategoryFormModal
        open={isModalOpen}
        title={editingCategory ? "Edit Category" : "Create Category"}
        initialValues={editingCategory}
        confirmLoading={isMutating}
        onCancel={handleCloseModal}
        onSubmit={handleSubmit}
      />
    </>
  );
}
