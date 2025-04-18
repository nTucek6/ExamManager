import { useState, useEffect } from "react";
import { ToastContainer, toast } from "react-toastify";
import "./HomePageAdmin.css";

import Select from "react-select";
import { BarLoader, ClipLoader } from "react-spinners";

import adminService from "../../../services/adminService";
import { useNavigate } from "react-router";

export default function HomePageAdmin() {
  const navigate = useNavigate();

  const [users, setUsers] = useState([]);
  const [roles, setRoles] = useState([]);

  const [loading, setLoading] = useState(true);

  const fetchUsers = async () => {
    const result = await adminService.getUser();
    setUsers(result.data);
    setLoading(false);
  };

  const fetchRoles = async () => {
    const result = await adminService.getUserRoles();
    setRoles(result.data);
  };

  useEffect(() => {
    fetchRoles();
  }, []);

  useEffect(() => {
    if (roles.length > 0) {
      fetchUsers();
    }
  }, [roles]);

  const handleRoleChange = async (role, userId) => {
    await adminService.changeUserRole(userId, role.value).then(() => {
      toast.success("Uspješno ste promjenili ulogu korisnika! ✅", {
        position: "top-right",
        autoClose: 3000,
        theme: "colored",
      });
    });
  };

  const handleLogout = () => {
    sessionStorage.clear();
    navigate("/login");
  };

  return (
    <>
      <nav className="navbar">
        <div className="navbar-container">
        <h3>Admin</h3>
          <button className="logout-button" onClick={handleLogout}>
            Odjava
          </button>
        </div>
      </nav>
      <div className="custom-wrapper">
      {loading ? (
        <ClipLoader />
      ) : users.length > 0 ? (
        
        <table id="custom-user-table">
          <thead>
            <tr>
              <th>Email</th>
              <th>Role</th>
            </tr>
          </thead>
          <tbody>
            {users.map((user) => {
              return (
                <tr key={user.Id}>
                  <td>{user.Email}</td>
                  <td>
                    {" "}
                    <Select
                      options={roles}
                      defaultValue={roles.find(
                        (role) => role.value === user.RoleId
                      )}
                      onChange={(data) => handleRoleChange(data, user.Id)}
                    />
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
        
      ) : (
        <h1>Nema dostupnih korisnika!</h1>
      )}
      </div>
      <ToastContainer />
    </>
  );
  /*
  return loading ? (
    <ClipLoader />
  ) : users.length > 0 ? (
    <>
      <nav>
        <button onClick={handleLogout}>Odjava</button>
      </nav>
      <table>
        <thead>
          <tr>
            <th>Email</th>
            <th>Role</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => {
            return (
              <tr key={user.Id}>
                <td>{user.Email}</td>
                <td>
                  {" "}
                  <Select
                    options={roles}
                    defaultValue={roles.find(
                      (role) => role.value === user.RoleId
                    )}
                    onChange={(data) => handleRoleChange(data, user.Id)}
                  />
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
      <ToastContainer />
    </>
  ) : (
    <h1>Nema dostupnih korisnika!</h1>
  );
  */
}
