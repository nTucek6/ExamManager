import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { ClipLoader } from "react-spinners";

import examService from "../../../services/examService";
import "./ExamStudents.css"

export default function ExamStudents() {
  const { id } = useParams();

  const [examStudents, setExamStudents] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchStudents = async () => {
    const result = await examService.getProfessorExamStudents(id);
    setExamStudents(result.data);
      setLoading(false);

  };

  useEffect(() => {
    fetchStudents()
  }, [id]);

  return (
    <div id="student-exam-container">
      {loading ? (
        <ClipLoader />
      ) : examStudents.length > 0 ? (
        examStudents.map((student) => {
          return (
            <ul key={student.Email}>
              <li>Email: {student.Email}</li>
              <li>Student: {student.Name} {student.Surname}</li>
            </ul>
          );
        })
      ) : (
        <h1>Trenutno nijedan student nije upisao ovaj rok!</h1>
      )}
    </div>
  );
}
