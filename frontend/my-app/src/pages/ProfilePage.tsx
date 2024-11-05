
import Sidebar from "../components/Sidebar";
import ProfileCard from "../components/profile/ProfileCard";
export default function ProfilePage() {
    document.body.style.background = '#FEFAF6';
    return(
        <div className="container">
            <div className="sidebar-container">
                <Sidebar/>
            </div>
            
            <div className="profile">
                <ProfileCard/>
            </div>
        </div>
        );
}