import { useEffect, useRef, useState } from "react";
import {
  Button,
  Card,
  Form,
  Input,
  message,
  Select,
  Space,
  Typography,
} from "antd";
import { ArrowLeftOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { selectTicketSubmitting } from "../ticketSelectors";
import { createTicketThunk } from "../ticketThunks";
import {
  type CreateTicketRequest,
  TicketPriority,
  ticketPriorityLabels,
} from "../ticketTypes";
import {
  ticketOptionsApi,
  type TicketCategoryOption,
} from "../ticketOptionsApi";

const { Title } = Typography;

type MaybePagedResult<T> = {
  items?: T[];
};

type MaybeApiResponse<T> = {
  data?: T | MaybePagedResult<T>;
};

type MaybeNestedApiResponse<T> = {
  data?: {
    data?: T[] | MaybePagedResult<T>;
  };
};

function extractCategories(response: unknown): TicketCategoryOption[] {
  console.log("getCategories raw response:", response);

  if (Array.isArray(response)) {
    return response as TicketCategoryOption[];
  }

  const maybeApiResponse = response as MaybeApiResponse<TicketCategoryOption[]>;

  if (Array.isArray(maybeApiResponse.data)) {
    return maybeApiResponse.data;
  }

  const maybePagedData = maybeApiResponse.data as
    | MaybePagedResult<TicketCategoryOption>
    | undefined;

  if (Array.isArray(maybePagedData?.items)) {
    return maybePagedData.items;
  }

  const maybeDirectPaged = response as MaybePagedResult<TicketCategoryOption>;

  if (Array.isArray(maybeDirectPaged.items)) {
    return maybeDirectPaged.items;
  }

  const maybeNested = response as MaybeNestedApiResponse<TicketCategoryOption>;

  if (Array.isArray(maybeNested.data?.data)) {
    return maybeNested.data.data;
  }

  const maybeNestedPaged = maybeNested.data?.data as
    | MaybePagedResult<TicketCategoryOption>
    | undefined;

  if (Array.isArray(maybeNestedPaged?.items)) {
    return maybeNestedPaged.items;
  }

  return [];
}

export default function TicketCreatePage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const isSubmitting = useAppSelector(selectTicketSubmitting);

  const [form] = Form.useForm<CreateTicketRequest>();
  const [categories, setCategories] = useState<TicketCategoryOption[]>([]);
  const [isCategoryLoading, setIsCategoryLoading] = useState(false);

  const didLoadCategoriesRef = useRef(false);

  useEffect(() => {
    if (didLoadCategoriesRef.current) {
      return;
    }

    didLoadCategoriesRef.current = true;

    const loadCategories = async () => {
      try {
        setIsCategoryLoading(true);

        const response = await ticketOptionsApi.getCategories();
        const extractedCategories = extractCategories(response);

        if (extractedCategories.length === 0) {
          console.warn("Invalid categories response shape:", response);
        }

        setCategories(extractedCategories);
      } catch (error) {
        console.error("Failed to load categories:", error);
        setCategories([]);
        message.error("Failed to load categories.");
      } finally {
        setIsCategoryLoading(false);
      }
    };

    loadCategories();
  }, []);

  const handleSubmit = async (values: CreateTicketRequest) => {
    try {
      const ticketId = await dispatch(createTicketThunk(values)).unwrap();

      message.success("Ticket created successfully.");
      navigate(`/app/tickets/${ticketId}`);
    } catch (error) {
      console.error("Create ticket failed:", error);
      message.error("Failed to create ticket.");
    }
  };

  const priorityOptions = Object.values(TicketPriority)
    .filter((value) => typeof value === "number")
    .map((value) => ({
      value,
      label: ticketPriorityLabels[value as TicketPriority],
    }));

  const categoryOptions = categories.map((category) => ({
    value: category.id,
    label: category.name,
  }));

  return (
    <Space direction="vertical" size="large" style={{ width: "100%" }}>
      <Space>
        <Button
          icon={<ArrowLeftOutlined />}
          onClick={() => navigate("/app/tickets")}
        >
          Back
        </Button>

        <Title level={3} style={{ margin: 0 }}>
          Create Ticket
        </Title>
      </Space>

      <Card>
        <Form
          form={form}
          layout="vertical"
          onFinish={handleSubmit}
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
              loading={isCategoryLoading}
              placeholder="Select category"
              options={categoryOptions}
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
            <Select placeholder="Select priority" options={priorityOptions} />
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
            <Input.TextArea
              rows={6}
              placeholder="Describe the issue..."
              maxLength={2000}
              showCount
            />
          </Form.Item>

          <Form.Item>
            <Space>
              <Button type="primary" htmlType="submit" loading={isSubmitting}>
                Create
              </Button>

              <Button onClick={() => navigate("/app/tickets")}>Cancel</Button>
            </Space>
          </Form.Item>
        </Form>
      </Card>
    </Space>
  );
}
