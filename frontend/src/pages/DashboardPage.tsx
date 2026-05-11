import { Card, Typography } from "antd";
import { useAppSelector } from "../app/hooks";
import { selectCurrentUser } from "../features/auth/authSelectors";

function DashboardPage() {
  const currentUser = useAppSelector(selectCurrentUser);

  return (
    <Card>
      <Typography.Title level={3}>Dashboard</Typography.Title>

      <Typography.Paragraph>
        Welcome, {currentUser?.fullName}.
      </Typography.Paragraph>

      <Typography.Paragraph>
        Your role: {currentUser?.role}
      </Typography.Paragraph>
    </Card>
  );
}

export default DashboardPage;
