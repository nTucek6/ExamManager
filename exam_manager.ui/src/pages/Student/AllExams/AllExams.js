import { useEffect, useState } from "react";
import { format } from "date-fns";
import { ClipLoader } from "react-spinners";

import studentExamService from "../../../services/studentExamService";

export default function AllExams() {
  const [studentExams, setStudentExams] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchData = async () => {
    const result = await studentExamService.getAllStudentExams(
      Number(sessionStorage.getItem("userId"))
    );

    setStudentExams(result.data);
    setLoading(false);
  };

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <div id="exam-container">
      {loading ? (
        <ClipLoader />
      ) : studentExams.length > 0 ? (
        studentExams.map((exam) => {
          return (
            <ul key={exam.ExamId}>
              <li>{exam.SubjectName}</li>
              <li>
                Datum roka:{" "}
                {format(new Date(exam.DeadlineDate), "dd.MM.yyyy HH:mm:ss")}
              </li>
              <li>
                Prijave do:{" "}
                {format(new Date(exam.ApplicationsDate), "dd.MM.yyyy HH:mm:ss")}
              </li>
              <li>
                Odjave do:{" "}
                {format(new Date(exam.CheckOutDate), "dd.MM.yyyy HH:mm:ss")}
              </li>
            </ul>
          );
        })
      ) : (
        <h1>Student trenutno nema postojećih ispita</h1>
      )}
    </div>
  );
}
