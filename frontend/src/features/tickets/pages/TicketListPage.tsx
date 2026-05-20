import { useEffect, useState } from "react";
import {
  Button,
  Card,
  Form,
  Select,
  Space,
  Table,
  Typography,
} from "antd";
import type { ColumnsType } from "antd/es/table";
import { PlusOutlined, ReloadOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import dayjs from "dayjs";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import {
  selectTicketLoading,
  selectTicketPagination,
  selectTickets,
} from "../ticketSelectors";
import { getTicketsThunk } from "../ticketThunks";
import {
  type Ticket,
  TicketPriority,
  ticketPriorityLabels,
  TicketStatus,
  ticketStatusLabels,
} from "../ticketTypes";
import TicketStatusTag from "../components/TicketStatusTag";
import TicketPriorityTag from "../components/TicketPriorityTag";

const { Title } = Typography;

interface TicketFilterValues {
  status?: TicketStatus;
  priority?: TicketPriority;
}

export default function TicketListPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const tickets = useAppSelector(selectTickets);
  const isLoading = useAppSelector(selectTicketLoading);
  const pagination = useAppSelector(selectTicketPagination);

  const [form] = Form.useForm<TicketFilterValues>();

  const [filters, setFilters] = useState<TicketFilterValues>({});
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(10);

  useEffect(() => {
    dispatch(
      getTicketsThunk({
        pageNumber,
        pageSize,
        status: filters.status ?? null,
        priority: filters.priority ?? null,
      }),
    );
  }, [dispatch, pageNumber, pageSize, filters.status, filters.priority]);

  const handleSearch = () => {
    const values = form.getFieldsValue();

    setPageNumber(1);
    setFilters(values);
  };

  const handleReset = () => {
    form.resetFields();

    setPageNumber(1);
    setFilters({});
  };

  const handleReload = () => {
    dispatch(
      getTicketsThunk({
        pageNumber,
        pageSize,
        status: filters.status ?? null,
        priority: filters.priority ?? null,
      }),
    );
  };

  const columns: ColumnsType<Ticket> = [
    {
      title: "Title",
      dataIndex: "title",
      key: "title",
      render: (_, record) => (
        <Button
          type="link"
          style={{ padding: 0 }}
          onClick={() => navigate(`/app/tickets/${record.id}`)}
        >
          {record.title}
        </Button>
      ),
    },
    {
      title: "Category",
      dataIndex: "categoryName",
      key: "categoryName",
    },
    {
      title: "Status",
      dataIndex: "status",
      key: "status",
      render: (status: TicketStatus) => <TicketStatusTag status={status} />,
    },
    {
      title: "Priority",
      dataIndex: "priority",
      key: "priority",
      render: (priority: TicketPriority) => (
        <TicketPriorityTag priority={priority} />
      ),
    },
    {
      title: "Assigned To",
      dataIndex: "assignedToUserName",
      key: "assignedToUserName",
      render: (_: string | null, record) =>
        record.assignedToUserName ?? "Unassigned",
    },
    {
      title: "Created At",
      dataIndex: "createdAtUtc",
      key: "createdAtUtc",
      render: (value: string) => dayjs(value).format("DD/MM/YYYY HH:mm"),
    },
  ];

  return (
    <Space direction="vertical" size="large" style={{ width: "100%" }}>
      <Space
        align="center"
        style={{
          width: "100%",
          justifyContent: "space-between",
        }}
      >
        <Title level={3} style={{ margin: 0 }}>
          Tickets
        </Title>

        <Button
          type="primary"
          icon={<PlusOutlined />}
          onClick={() => navigate("/app/tickets/create")}
        >
          Create Ticket
        </Button>
      </Space>

      <Card>
        <Form form={form} layout="inline" onFinish={handleSearch}>
          <Form.Item name="status" label="Status">
            <Select
              allowClear
              placeholder="Select status"
              style={{ width: 180 }}
              options={Object.values(TicketStatus)
                .filter((value) => typeof value === "number")
                .map((value) => ({
                  value,
                  label: ticketStatusLabels[value as TicketStatus],
                }))}
            />
          </Form.Item>

          <Form.Item name="priority" label="Priority">
            <Select
              allowClear
              placeholder="Select priority"
              style={{ width: 180 }}
              options={Object.values(TicketPriority)
                .filter((value) => typeof value === "number")
                .map((value) => ({
                  value,
                  label: ticketPriorityLabels[value as TicketPriority],
                }))}
            />
          </Form.Item>

          <Form.Item>
            <Space>
              <Button type="primary" htmlType="submit">
                Filter
              </Button>

              <Button onClick={handleReset}>Reset</Button>

              <Button icon={<ReloadOutlined />} onClick={handleReload}>
                Reload
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Card>

      <Card>
        <Table
          rowKey="id"
          loading={isLoading}
          columns={columns}
          dataSource={tickets}
          pagination={{
            current: pagination.pageNumber,
            pageSize: pagination.pageSize,
            total: pagination.totalCount,
            showSizeChanger: true,
            onChange: (page, size) => {
              setPageNumber(page);
              setPageSize(size);
            },
          }}
        />
      </Card>
    </Space>
  );
}