import { toast } from "react-toastify";

export default function ToastInfo(Message) {
  toast.success(Message + " ✅", {
    position: "top-right",
    autoClose: 3000,
    theme: "colored",
  });
}
