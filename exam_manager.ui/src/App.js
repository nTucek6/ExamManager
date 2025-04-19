import "./App.css";
import "./css/form.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/NavbarComponent"; // Import the Navbar
import LimitedRoutes from "./utilities/LimitedRoutes";
import Login from "./pages/Auth/Login/LoginPage";
import Register from "./pages/Auth/Register/RegisterPage.js";
import Dashboard from "./pages/Home/DashboardPage";
import CheckUserLogin from "./utilities/CheckUserLogin";
import Exams from "./pages/Student/Exams/ExamsPage";
import CheckProfessorRole from "./utilities/CheckProfessorRole";
import ExamRegistration from "./pages/Student/AddExam/ExamRegistration.js";
import ChangePasswordPage from "./pages/Auth/ChangePassword/ChangePassword.js";
import CreateExam from "./pages/Professor/CreateExam/CreateExam.js";
import CheckStudentRole from "./utilities/CheckStudentRole.js";
import ProfessorExams from "./pages/Professor/ProfessorExams/ProfessorExams.js";
import ExamStudents from "./pages/Professor/ExamStudents/ExamStudents.js";
import AllExams from "./pages/Student/AllExams/AllExams.js";
import Scheduler from "./components/Scheduler";
import About from "./pages/About/About.js";
import HomePageAdmin from "./pages/Admin/HomePage/HomePageAdmin.js";
import CheckAdminRole from "./utilities/CheckAdminRole.js";
import ForgotPassword from "./pages/Auth/ForgotPassword/ForgotPasswordPage.js";
import RestartPassword from "./pages/Auth/RestartPassword/RestartPassword.js";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* If user is not logged in, restrict these routes */}
        <Route element={<CheckUserLogin />}>
          {/* Navbar is parent because of maintainability */}
          <Route path="/" element={<Navbar />}>
            <Route path="/" element={<Scheduler />} />
            <Route path="/about" element={<About />} />

            <Route path="/change-password" element={<ChangePasswordPage />} />

            {/* Student routes */}
            <Route element={<CheckStudentRole />}>
              <Route path="/exams" element={<Exams />} />
              <Route path="/register-exam" element={<ExamRegistration />} />
              <Route path="/all-exams" element={<AllExams />} />
            </Route>

            {/* Professor routes */}
            <Route element={<CheckProfessorRole />}>
              <Route path="/exams-professor" element={<ProfessorExams />} />
              <Route path="/create-exam/:id?" element={<CreateExam />} />
              <Route path="/exam-students/:id" element={<ExamStudents />} />
            </Route>
          </Route>

          {/* Admin routes */}
          <Route element={<CheckAdminRole />}>
            <Route path="/admin" element={<HomePageAdmin />} />
          </Route>
          
        </Route>

        {/* If user is logged in, restrict these routes */}
        <Route element={<LimitedRoutes />}>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/forgot-password" element={<ForgotPassword />} />
          <Route path="/restart-password/:token" element={<RestartPassword />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
