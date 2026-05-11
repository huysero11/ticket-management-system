import { Button, Card, Form, Input, Typography, message } from "antd";
import { Link, useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { selectAuthErrorMessage, selectAuthLoading } from "../authSelectors";
import { loginThunk } from "../authSlice";
import type { LoginRequest } from "../authTypes";

function LoginPage() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const isLoading = useAppSelector(selectAuthLoading);
  const errorMessage = useAppSelector(selectAuthErrorMessage);

  const handleSubmit = async (values: LoginRequest) => {
    console.log("Login form submitted:", values);
    const result = await dispatch(loginThunk(values));

    if (loginThunk.fulfilled.match(result)) {
      message.success("Login successfully.");
      navigate("/app/dashboard", { replace: true });
      return;
    }

    message.error(result.payload?.message || "Login failed.");
  };

  return (
    <Card className="auth-card">
      <Typography.Title level={3} className="auth-title">
        Login
      </Typography.Title>

      {errorMessage && (
        <Typography.Text type="danger" className="auth-error">
          {errorMessage}
        </Typography.Text>
      )}

      <Form layout="vertical" onFinish={handleSubmit}>
        <Form.Item<LoginRequest>
          label="Email"
          name="email"
          rules={[
            { required: true, message: "Email is required." },
            { type: "email", message: "Email is invalid." },
          ]}
        >
          <Input placeholder="Enter your email" />
        </Form.Item>

        <Form.Item<LoginRequest>
          label="Password"
          name="password"
          rules={[{ required: true, message: "Password is required." }]}
        >
          <Input.Password placeholder="Enter your password" />
        </Form.Item>

        <Button type="primary" htmlType="submit" loading={isLoading} block>
          Login
        </Button>
      </Form>

      <Typography.Paragraph className="auth-bottom-text">
        Do not have an account? <Link to="/register">Register</Link>
      </Typography.Paragraph>
    </Card>
  );
}

export default LoginPage;
