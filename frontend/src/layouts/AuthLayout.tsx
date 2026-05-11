import { Outlet } from "react-router-dom";

function AuthLayout() {
  return (
    <div className="full-page-center">
      <Outlet />
    </div>
  );
}

export default AuthLayout;
