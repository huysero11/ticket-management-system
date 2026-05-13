import { useEffect, useState } from "react";
import {
  Button,
  Card,
  Descriptions,
  Divider,
  Form,
  Input,
  message,
  Select,
  Space,
  Spin,
  Typography,
} from "antd";
import { ArrowLeftOutlined, SaveOutlined } from "@ant-design/icons";
import { useNavigate, useParams } from "react-router-dom";
import dayjs from "dayjs";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { selectCurrentUser } from "../../auth/authSelectors";
import {
  selectSelectedTicket,
  selectTicketDetailLoading,
  selectTicketSubmitting,
} from "../ticketSelectors";
import {
  assignTicketThunk,
  changeTicketStatusThunk,
  getTicketByIdThunk,
  updateTicketThunk,
} from "../ticketThunks";
import {
  TicketPriority,
  ticketPriorityLabels,
  TicketStatus,
  ticketStatusLabels,
  type UpdateTicketRequest,
} from "../ticketTypes";
import TicketPriorityTag from "../components/TicketPriorityTag";
import TicketStatusTag from "../components/TicketStatusTag";
import TicketCommentSection from "../components/TicketCommentSection";
import {
  ticketOptionsApi,
  type AgentOption,
  type TicketCategoryOption,
} from "../ticketOptionsApi";

const { Title, Paragraph } = Typography;

export default function TicketDetailPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const params = useParams();

  const ticketId = params.id;

  const currentUser = useAppSelector(selectCurrentUser);
  const isAdmin = currentUser?.role === "Admin";

  const ticket = useAppSelector(selectSelectedTicket);
  const isDetailLoading = useAppSelector(selectTicketDetailLoading);
  const isSubmitting = useAppSelector(selectTicketSubmitting);

  const [form] = Form.useForm<UpdateTicketRequest>();

  const [categories, setCategories] = useState<TicketCategoryOption[]>([]);
  const [agents, setAgents] = useState<AgentOption[]>([]);

  const [isEditing, setIsEditing] = useState(false);
  const [selectedStatus, setSelectedStatus] = useState<
    TicketStatus | undefined
  >();
  const [selectedAgentId, setSelectedAgentId] = useState<string | undefined>();

  useEffect(() => {
    if (!ticketId) {
      return;
    }

    dispatch(getTicketByIdThunk(ticketId));
  }, [dispatch, ticketId]);

  useEffect(() => {
    const loadOptions = async () => {
      try {
        const categoryResponse = await ticketOptionsApi.getCategories();
        setCategories(categoryResponse.data);
      } catch {
        message.error("Failed to load categories.");
      }

      if (!isAdmin) {
        return;
      }

      try {
        const agentResponse = await ticketOptionsApi.getAgents();
        setAgents(agentResponse.data);
      } catch {
        setAgents([]);
        message.error("Failed to load agents.");
      }
    };

    loadOptions();
  }, [isAdmin]);

  useEffect(() => {
    if (!ticket || !isEditing) {
      return;
    }

    form.setFieldsValue({
      title: ticket.title,
      description: ticket.description,
      categoryId: ticket.categoryId,
      priority: ticket.priority,
    });
  }, [form, ticket, isEditing]);

  const reloadTicket = () => {
    if (!ticketId) {
      return;
    }

    dispatch(getTicketByIdThunk(ticketId));
  };

  const handleUpdateTicket = async (values: UpdateTicketRequest) => {
    if (!ticketId) {
      return;
    }

    try {
      await dispatch(
        updateTicketThunk({
          id: ticketId,
          payload: values,
        }),
      ).unwrap();

      message.success("Ticket updated successfully.");
      setIsEditing(false);
      reloadTicket();
    } catch {
      message.error("Failed to update ticket.");
    }
  };

  const handleAssignTicket = async () => {
    if (!ticketId) {
      return;
    }

    const agentId = selectedAgentId ?? ticket?.assignedToUserId ?? undefined;

    if (!agentId) {
      message.warning("Please select an agent.");
      return;
    }

    try {
      await dispatch(
        assignTicketThunk({
          id: ticketId,
          payload: {
            assignedToUserId: agentId,
          },
        }),
      ).unwrap();

      message.success("Ticket assigned successfully.");
      setSelectedAgentId(undefined);
      reloadTicket();
    } catch {
      message.error("Failed to assign ticket.");
    }
  };

  const handleChangeStatus = async () => {
    if (!ticketId || !ticket) {
      return;
    }

    const status = selectedStatus ?? ticket.status;

    try {
      await dispatch(
        changeTicketStatusThunk({
          id: ticketId,
          payload: {
            status,
          },
        }),
      ).unwrap();

      message.success("Ticket status changed successfully.");
      setSelectedStatus(undefined);
      reloadTicket();
    } catch {
      message.error("Failed to change ticket status.");
    }
  };

  if (isDetailLoading) {
    return (
      <Card>
        <Spin />
      </Card>
    );
  }

  if (!ticket) {
    return (
      <Card>
        <Paragraph>Ticket not found.</Paragraph>
        <Button onClick={() => navigate("/app/tickets")}>
          Back to Tickets
        </Button>
      </Card>
    );
  }

  return (
    <Space orientation="vertical" size="large" style={{ width: "100%" }}>
      <Space
        align="center"
        style={{
          width: "100%",
          justifyContent: "space-between",
        }}
      >
        <Space>
          <Button
            icon={<ArrowLeftOutlined />}
            onClick={() => navigate("/app/tickets")}
          >
            Back
          </Button>

          <Title level={3} style={{ margin: 0 }}>
            Ticket Detail
          </Title>
        </Space>

        <Button onClick={() => setIsEditing((value) => !value)}>
          {isEditing ? "Cancel Edit" : "Edit Ticket"}
        </Button>
      </Space>

      <Card>
        <Descriptions bordered column={2}>
          <Descriptions.Item label="Title" span={2}>
            {ticket.title}
          </Descriptions.Item>

          <Descriptions.Item label="Status">
            <TicketStatusTag status={ticket.status} />
          </Descriptions.Item>

          <Descriptions.Item label="Priority">
            <TicketPriorityTag priority={ticket.priority} />
          </Descriptions.Item>

          <Descriptions.Item label="Category">
            {ticket.categoryName}
          </Descriptions.Item>

          <Descriptions.Item label="Assigned To">
            {ticket.assignedToUserName ??
              ticket.assignedToUserId ??
              "Unassigned"}
          </Descriptions.Item>

          <Descriptions.Item label="Created By">
            {ticket.createdByUserName ?? ticket.createdByUserId}
          </Descriptions.Item>

          <Descriptions.Item label="Created At">
            {dayjs(ticket.createdAtUtc).format("DD/MM/YYYY HH:mm")}
          </Descriptions.Item>

          <Descriptions.Item label="Updated At">
            {ticket.updatedAtUtc
              ? dayjs(ticket.updatedAtUtc).format("DD/MM/YYYY HH:mm")
              : "-"}
          </Descriptions.Item>

          <Descriptions.Item label="Closed At">
            {ticket.closedAtUtc
              ? dayjs(ticket.closedAtUtc).format("DD/MM/YYYY HH:mm")
              : "-"}
          </Descriptions.Item>

          <Descriptions.Item label="Description" span={2}>
            {ticket.description}
          </Descriptions.Item>
        </Descriptions>
      </Card>

      <Card title="Actions">
        <Space orientation="vertical" size="middle" style={{ width: "100%" }}>
          {isAdmin && (
            <Space wrap>
              <Select
                placeholder="Assign to agent"
                style={{ width: 280 }}
                value={selectedAgentId ?? ticket.assignedToUserId ?? undefined}
                onChange={setSelectedAgentId}
                options={agents.map((agent) => ({
                  value: agent.id,
                  label: `${agent.fullName} (${agent.email})`,
                }))}
              />

              <Button
                type="primary"
                loading={isSubmitting}
                onClick={handleAssignTicket}
              >
                Assign
              </Button>
            </Space>
          )}

          <Space wrap>
            <Select
              placeholder="Change status"
              style={{ width: 220 }}
              value={selectedStatus ?? ticket.status}
              onChange={setSelectedStatus}
              options={Object.values(TicketStatus)
                .filter((value) => typeof value === "number")
                .map((value) => ({
                  value,
                  label: ticketStatusLabels[value as TicketStatus],
                }))}
            />

            <Button
              type="primary"
              loading={isSubmitting}
              onClick={handleChangeStatus}
            >
              Change Status
            </Button>
          </Space>
        </Space>
      </Card>

      {isEditing && (
        <Card title="Edit Ticket">
          <Form
            form={form}
            layout="vertical"
            onFinish={handleUpdateTicket}
            style={{ maxWidth: 720 }}
          >
            <Form.Item
              name="title"
              label="Title"
              rules={[
                {
                  required: true,
                  message: "Please enter ticket title.",
                },
                {
                  whitespace: true,
                  message: "Title cannot be empty.",
                },
              ]}
            >
              <Input placeholder="Enter ticket title" />
            </Form.Item>

            <Form.Item
              name="categoryId"
              label="Category"
              rules={[
                {
                  required: true,
                  message: "Please select category.",
                },
              ]}
            >
              <Select
                placeholder="Select category"
                options={categories.map((category) => ({
                  value: category.id,
                  label: category.name,
                }))}
              />
            </Form.Item>

            <Form.Item
              name="priority"
              label="Priority"
              rules={[
                {
                  required: true,
                  message: "Please select priority.",
                },
              ]}
            >
              <Select
                placeholder="Select priority"
                options={Object.values(TicketPriority)
                  .filter((value) => typeof value === "number")
                  .map((value) => ({
                    value,
                    label: ticketPriorityLabels[value as TicketPriority],
                  }))}
              />
            </Form.Item>

            <Form.Item
              name="description"
              label="Description"
              rules={[
                {
                  required: true,
                  message: "Please enter description.",
                },
                {
                  whitespace: true,
                  message: "Description cannot be empty.",
                },
              ]}
            >
              <Input.TextArea rows={6} maxLength={2000} showCount />
            </Form.Item>

            <Form.Item>
              <Space>
                <Button
                  type="primary"
                  htmlType="submit"
                  loading={isSubmitting}
                  icon={<SaveOutlined />}
                >
                  Save
                </Button>

                <Button onClick={() => setIsEditing(false)}>Cancel</Button>
              </Space>
            </Form.Item>
          </Form>
        </Card>
      )}

      <Divider />

      <TicketCommentSection ticketId={ticket.id} />
    </Space>
  );
}
