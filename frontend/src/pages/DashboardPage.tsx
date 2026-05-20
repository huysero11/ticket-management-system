import { useEffect } from "react";
import {
  Card,
  Col,
  Empty,
  Row,
  Space,
  Spin,
  Statistic,
  Table,
  Typography,
  message,
} from "antd";
import type { ColumnsType } from "antd/es/table";
import { useNavigate } from "react-router-dom";
import dayjs from "dayjs";
import { useAppDispatch, useAppSelector } from "../app/hooks";
import {
  selectDashboardError,
  selectDashboardLoading,
  selectDashboardSummary,
} from "../features/dashboard/dashboardSelectors";
import { getDashboardSummaryThunk } from "../features/dashboard/dashboardThunks";
import type { DashboardRecentTicket } from "../features/dashboard/dashboardTypes";
import {
  TicketPriority,
  TicketStatus,
} from "../features/tickets/ticketTypes";
import TicketStatusTag from "../features/tickets/components/TicketStatusTag";
import TicketPriorityTag from "../features/tickets/components/TicketPriorityTag";

const { Title, Text } = Typography;

export default function DashboardPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const summary = useAppSelector(selectDashboardSummary);
  const isLoading = useAppSelector(selectDashboardLoading);
  const error = useAppSelector(selectDashboardError);

  useEffect(() => {
    dispatch(getDashboardSummaryThunk());
  }, [dispatch]);

  useEffect(() => {
    if (error) {
      message.error(error);
    }
  }, [error]);

  const columns: ColumnsType<DashboardRecentTicket> = [
    {
      title: "Title",
      dataIndex: "title",
      key: "title",
      render: (_, record) => (
        <Text
          style={{ cursor: "pointer" }}
          onClick={() => navigate(`/app/tickets/${record.id}`)}
        >
          {record.title}
        </Text>
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

  if (isLoading && !summary) {
    return (
      <Card>
        <Spin />
      </Card>
    );
  }

  if (!summary) {
    return (
      <Card>
        <Empty description="No dashboard data." />
      </Card>
    );
  }

  return (
    <Space direction="vertical" size="large" style={{ width: "100%" }}>
      <Title level={3} style={{ margin: 0 }}>
        Dashboard
      </Title>

      <Row gutter={[16, 16]}>
        <Col xs={24} sm={12} lg={8} xl={4}>
          <Card>
            <Statistic title="Total Tickets" value={summary.totalTickets} />
          </Card>
        </Col>

        <Col xs={24} sm={12} lg={8} xl={5}>
          <Card>
            <Statistic title="Open" value={summary.openTickets} />
          </Card>
        </Col>

        <Col xs={24} sm={12} lg={8} xl={5}>
          <Card>
            <Statistic
              title="In Progress"
              value={summary.inProgressTickets}
            />
          </Card>
        </Col>

        <Col xs={24} sm={12} lg={8} xl={5}>
          <Card>
            <Statistic title="Resolved" value={summary.resolvedTickets} />
          </Card>
        </Col>

        <Col xs={24} sm={12} lg={8} xl={5}>
          <Card>
            <Statistic title="Closed" value={summary.closedTickets} />
          </Card>
        </Col>
      </Row>

      <Card title="Recent Tickets">
        <Table
          rowKey="id"
          loading={isLoading}
          columns={columns}
          dataSource={summary.recentTickets}
          pagination={false}
        />
      </Card>
    </Space>
  );
}