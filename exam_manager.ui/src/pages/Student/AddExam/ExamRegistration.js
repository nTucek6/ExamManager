import { useEffect, useState } from "react";
import { format } from "date-fns";
import { ClipLoader } from "react-spinners";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import studentExamService from "../../../services/studentExamService";

export default function ExamRegistration() {
  const [exams, setExams] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
    const result = await studentExamService.getExamsForRegister(
      Number(sessionStorage.getItem("userId"))
    );
    setExams(result.data);
    setLoading(false);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleExamRegister = async (examId) => {
    await studentExamService.registerStudentExam(
      Number(sessionStorage.getItem("userId")),
      examId
    );
    toast.success("Ispit uspješno prijavljen! ✅", {
      position: "top-right",
      autoClose: 3000,
      theme: "colored",
    });
    fetchData();
  };

  return (
    <>
      <div id="exam-container">
        {loading ? (
          <ClipLoader />
        ) : exams.length > 0 ? (
          exams.map((exam) => {
            return (
              <ul key={exam.ExamId}>
                <li>{exam.SubjectName}</li>
                <li>
                  Datum roka:{" "}
                  {format(new Date(exam.DeadlineDate), "dd.MM.yyyy HH:mm:ss")}
                </li>
                <li>
                  Prijave do:{" "}
                  {format(
                    new Date(exam.ApplicationsDate),
                    "dd.MM.yyyy HH:mm:ss"
                  )}
                </li>
                <li>
                  Odjave do:{" "}
                  {format(new Date(exam.CheckOutDate), "dd.MM.yyyy HH:mm:ss")}
                </li>
                <button
                  type="button"
                  onClick={() => handleExamRegister(exam.ExamId)}
                >
                  Prijavi ispit
                </button>
              </ul>
            );
          })
        ) : (
          <h1>Student trenutno nema mogućnost upisati ispite</h1>
        )}
      </div>
      <ToastContainer />
    </>
  );
}
