import { Form, Input, Modal } from "antd";
import { useEffect } from "react";
import type {
  CreateTicketCategoryRequest,
  TicketCategory,
} from "../ticketCategoryTypes";

const { TextArea } = Input;

interface TicketCategoryFormModalProps {
  open: boolean;
  title: string;
  initialValues?: TicketCategory | null;
  confirmLoading?: boolean;
  onCancel: () => void;
  onSubmit: (values: CreateTicketCategoryRequest) => void | Promise<void>;
}

export default function TicketCategoryFormModal({
  open,
  title,
  initialValues,
  confirmLoading = false,
  onCancel,
  onSubmit,
}: TicketCategoryFormModalProps) {
  const [form] = Form.useForm<CreateTicketCategoryRequest>();

  useEffect(() => {
    if (!open) {
      form.resetFields();
      return;
    }

    if (initialValues) {
      form.setFieldsValue({
        name: initialValues.name,
        description: initialValues.description ?? "",
      });
    } else {
      form.resetFields();
    }
  }, [form, initialValues, open]);

  return (
    <Modal
      title={title}
      open={open}
      onCancel={onCancel}
      onOk={() => form.submit()}
      confirmLoading={confirmLoading}
      destroyOnHidden
    >
      <Form
        form={form}
        layout="vertical"
        onFinish={(values) => {
          onSubmit({
            name: values.name.trim(),
            description: values.description?.trim() || null,
          });
        }}
      >
        <Form.Item
          label="Name"
          name="name"
          rules={[
            {
              required: true,
              message: "Category name is required.",
            },
            {
              max: 100,
              message: "Category name must not exceed 100 characters.",
            },
          ]}
        >
          <Input placeholder="Example: Technical Support" />
        </Form.Item>

        <Form.Item
          label="Description"
          name="description"
          rules={[
            {
              max: 255,
              message: "Description must not exceed 255 characters.",
            },
          ]}
        >
          <TextArea
            rows={4}
            placeholder="Short description for this category"
          />
        </Form.Item>
      </Form>
    </Modal>
  );
}
