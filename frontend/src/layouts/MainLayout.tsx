import { Button, Layout, Menu, Typography } from "antd";
import type { MenuProps } from "antd";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { useMemo } from "react";
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { selectCurrentUser } from "../features/auth/authSelectors";
import { logoutThunk } from "../features/auth/authSlice";

const { Header, Sider, Content } = Layout;

function MainLayout() {
  const navigate = useNavigate();
  const location = useLocation();
  const dispatch = useAppDispatch();

  const currentUser = useAppSelector(selectCurrentUser);

  const isAdmin = currentUser?.role === "Admin";

  const handleLogout = async () => {
    await dispatch(logoutThunk());
    navigate("/login", { replace: true });
  };

  const menuItems: MenuProps["items"] = useMemo(
    () => [
      {
        key: "/app/dashboard",
        label: "Dashboard",
        onClick: () => navigate("/app/dashboard"),
      },
      ...(isAdmin
        ? [
            {
              key: "/app/ticket-categories",
              label: "Ticket Categories",
              onClick: () => navigate("/app/ticket-categories"),
            },
          ]
        : []),
    ],
    [isAdmin, navigate],
  );

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
            fontWeight: 700,
          }}
        >
          TMS
        </div>

        <Menu
          theme="dark"
          mode="inline"
          selectedKeys={[location.pathname]}
          items={menuItems}
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
