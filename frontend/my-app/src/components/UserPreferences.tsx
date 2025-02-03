import "../styles/profile.css";
import RecommendationDistanceSlider from "./RecommendationDistanceSlider";
import AppearenceMode from "./AppearenceMode";
export interface UserPreferencesProps {
    distance: number;
    onToggleShowPets: () => void;
    handleLogout: (event : React.MouseEvent<HTMLButtonElement>) => void;
    handleDeleteAccount: (event : React.MouseEvent<HTMLButtonElement>) => void;
  }

export default function UserPreferences(props : UserPreferencesProps) {
    return(
        <>
            <h3 className="sidebar-title">ACCOUNT SETTINGS</h3>
            <button className="preference-btn">Manage Payment Account</button>
            <h3 className="sidebar-title">DISCOVERY SETTINGS</h3>
            <RecommendationDistanceSlider/>
            <AppearenceMode/>

            <h3 className="sidebar-title">LEGAL</h3>
            
            <div className="legal-section">
                <a href="#Privacy Settings">Privacy Settings</a>
                <a href="#Cookie Policy">Cookie Policy</a>
                <a href="#Privacy Policy">Privacy Policy</a>
                <a href="#Terms of Service">Terms of Service</a>
            </div>
            <br/>
            <button className="preference-btn" onClick={props.handleLogout}>Log out</button>
            <button className="preference-btn" onClick={props.handleDeleteAccount}>Delete Account</button>
        </>
    );
}