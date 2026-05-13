import { useEffect, useState } from "react";
import {
  Button,
  Card,
  Empty,
  Form,
  Input,
  List,
  message,
  Popconfirm,
  Space,
  Typography,
} from "antd";
import dayjs from "dayjs";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import {
  addTicketCommentThunk,
  deleteTicketCommentThunk,
  getTicketCommentsThunk,
  updateTicketCommentThunk,
} from "../ticketThunks";
import {
  selectTicketCommentLoading,
  selectTicketComments,
} from "../ticketSelectors";

const { Text, Paragraph } = Typography;

interface TicketCommentSectionProps {
  ticketId: string;
}

interface CommentFormValues {
  message: string;
}

export default function TicketCommentSection({
  ticketId,
}: TicketCommentSectionProps) {
  const dispatch = useAppDispatch();

  const comments = useAppSelector(selectTicketComments);
  const isLoading = useAppSelector(selectTicketCommentLoading);

  const [form] = Form.useForm<CommentFormValues>();

  const [editingCommentId, setEditingCommentId] = useState<string | null>(null);
  const [editingMessage, setEditingMessage] = useState("");

  useEffect(() => {
    dispatch(getTicketCommentsThunk(ticketId));
  }, [dispatch, ticketId]);

  const handleAddComment = async (values: CommentFormValues) => {
    try {
      await dispatch(
        addTicketCommentThunk({
          ticketId,
          payload: {
            message: values.message,
          },
        }),
      ).unwrap();

      form.resetFields();
      message.success("Comment added successfully.");
    } catch {
      message.error("Failed to add comment.");
    }
  };

  const handleUpdateComment = async (commentId: string) => {
    try {
      await dispatch(
        updateTicketCommentThunk({
          ticketId,
          commentId,
          payload: {
            message: editingMessage,
          },
        }),
      ).unwrap();

      setEditingCommentId(null);
      setEditingMessage("");
      message.success("Comment updated successfully.");
    } catch {
      message.error("Failed to update comment.");
    }
  };

  const handleDeleteComment = async (commentId: string) => {
    try {
      await dispatch(
        deleteTicketCommentThunk({
          ticketId,
          commentId,
        }),
      ).unwrap();

      message.success("Comment deleted successfully.");
    } catch {
      message.error("Failed to delete comment.");
    }
  };

  return (
    <Card title="Comments">
      <Form form={form} layout="vertical" onFinish={handleAddComment}>
        <Form.Item
          name="message"
          rules={[
            {
              required: true,
              message: "Please enter your comment.",
            },
            {
              whitespace: true,
              message: "Comment cannot be empty.",
            },
          ]}
        >
          <Input.TextArea
            rows={3}
            placeholder="Write a comment..."
            maxLength={1000}
            showCount
          />
        </Form.Item>

        <Form.Item>
          <Button type="primary" htmlType="submit" loading={isLoading}>
            Add Comment
          </Button>
        </Form.Item>
      </Form>

      {comments.length === 0 ? (
        <Empty description="No comments yet." />
      ) : (
        <List
          loading={isLoading}
          dataSource={comments}
          renderItem={(comment) => (
            <List.Item
              actions={[
                <Button
                  key="edit"
                  type="link"
                  onClick={() => {
                    setEditingCommentId(comment.id);
                    setEditingMessage(comment.message);
                  }}
                >
                  Edit
                </Button>,
                <Popconfirm
                  key="delete"
                  title="Delete this comment?"
                  okText="Delete"
                  cancelText="Cancel"
                  okButtonProps={{ danger: true }}
                  onConfirm={() => handleDeleteComment(comment.id)}
                >
                  <Button type="link" danger>
                    Delete
                  </Button>
                </Popconfirm>,
              ]}
            >
              <List.Item.Meta
                title={
                  <Space orientation="vertical" size={0}>
                    <Text strong>{comment.userName ?? comment.userId}</Text>
                    <Text type="secondary">
                      {dayjs(comment.createdAtUtc).format("DD/MM/YYYY HH:mm")}
                    </Text>
                  </Space>
                }
                description={
                  editingCommentId === comment.id ? (
                    <Space orientation="vertical" style={{ width: "100%" }}>
                      <Input.TextArea
                        rows={3}
                        value={editingMessage}
                        onChange={(event) =>
                          setEditingMessage(event.target.value)
                        }
                      />

                      <Space>
                        <Button
                          type="primary"
                          size="small"
                          loading={isLoading}
                          onClick={() => handleUpdateComment(comment.id)}
                        >
                          Save
                        </Button>

                        <Button
                          size="small"
                          onClick={() => {
                            setEditingCommentId(null);
                            setEditingMessage("");
                          }}
                        >
                          Cancel
                        </Button>
                      </Space>
                    </Space>
                  ) : (
                    <Paragraph style={{ marginBottom: 0 }}>
                      {comment.message}
                    </Paragraph>
                  )
                }
              />
            </List.Item>
          )}
        />
      )}
    </Card>
  );
}
