import { App as AntdApp, Button, Card, Form, Input, Typography } from "antd";
import { Link, useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { selectAuthErrorMessage, selectAuthLoading } from "../authSelectors";
import { registerThunk } from "../authSlice";
import type { RegisterRequest } from "../authTypes";

function RegisterPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { message } = AntdApp.useApp();

  const isLoading = useAppSelector(selectAuthLoading);
  const errorMessage = useAppSelector(selectAuthErrorMessage);

  const handleSubmit = async (values: RegisterRequest) => {
    const result = await dispatch(registerThunk(values));

    if (registerThunk.fulfilled.match(result)) {
      message.success("Register successfully. Please login.");
      navigate("/login", { replace: true });
      return;
    }

    if (registerThunk.rejected.match(result)) {
      message.error(result.payload?.message ?? "Register failed.");
    }
  };

  return (
    <Card className="auth-card">
      <Typography.Title level={3} className="auth-title">
        Register
      </Typography.Title>

      {errorMessage && (
        <Typography.Text type="danger" className="auth-error">
          {errorMessage}
        </Typography.Text>
      )}

      <Form layout="vertical" onFinish={handleSubmit}>
        <Form.Item<RegisterRequest>
          label="Full name"
          name="fullName"
          rules={[{ required: true, message: "Full name is required." }]}
        >
          <Input placeholder="Enter your full name" />
        </Form.Item>

        <Form.Item<RegisterRequest>
          label="Email"
          name="email"
          rules={[
            { required: true, message: "Email is required." },
            { type: "email", message: "Email is invalid." },
          ]}
        >
          <Input placeholder="Enter your email" />
        </Form.Item>

        <Form.Item<RegisterRequest>
          label="Password"
          name="password"
          rules={[
            { required: true, message: "Password is required." },
            { min: 6, message: "Password must be at least 6 characters." },
          ]}
        >
          <Input.Password placeholder="Enter your password" />
        </Form.Item>

        <Button type="primary" htmlType="submit" loading={isLoading} block>
          Register
        </Button>
      </Form>

      <Typography.Paragraph className="auth-bottom-text">
        Already have an account? <Link to="/login">Login</Link>
      </Typography.Paragraph>
    </Card>
  );
}

export default RegisterPage;
