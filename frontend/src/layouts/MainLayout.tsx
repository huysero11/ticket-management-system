import { Button, Layout, Menu, Typography } from "antd";
import { Outlet, useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { selectCurrentUser } from "../features/auth/authSelectors";
import { logoutThunk } from "../features/auth/authSlice";

const { Header, Sider, Content } = Layout;

function MainLayout() {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const currentUser = useAppSelector(selectCurrentUser);

  const handleLogout = async () => {
    await dispatch(logoutThunk());
    navigate("/login", { replace: true });
  };

  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Sider>
        <div
          className="app-logo"
          style={{
            fontSize: "40px",
            color: "white",
            textAlign: "center",
            padding: "20px 0",
          }}
        >
          TMS
        </div>

        <Menu
          theme="dark"
          mode="inline"
          defaultSelectedKeys={["dashboard"]}
          items={[
            {
              key: "dashboard",
              label: "Dashboard",
              onClick: () => navigate("/app/dashboard"),
            },
          ]}
        />
      </Sider>

      <Layout>
        <Header
          className="app-header"
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
          }}
        >
          <Typography.Text strong>
            {currentUser?.fullName} ({currentUser?.role})
          </Typography.Text>

          <Button onClick={handleLogout}>Logout</Button>
        </Header>

        <Content className="app-content">
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  );
}

export default MainLayout;
