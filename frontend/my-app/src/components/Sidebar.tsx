import "../styles/profile.css"
import EmptyHeader from "./EmptyHeader";
interface SideBarProps {
    handleLogout :  (event: React.MouseEvent<HTMLButtonElement>) => void;
    handleDeleteAccount:  (event: React.MouseEvent<HTMLButtonElement>) => void;
}
export default function Sidebar(props : SideBarProps) {
    return(
        <div className="sidebar">
            <EmptyHeader/>
            <h3 className="sidebar-title">ACCOUNT SETTINGS</h3>
            <button className="preference-btn">Manage Payment Account</button>
            <h3 className="sidebar-title">DISCOVERY SETTINGS</h3>
            <div>
                <div className="distance-info">
                    <label htmlFor="distance-slider">Distance Preference</label>
                    <label htmlFor="distance-slider">13km</label>
                </div>
                <input id="distance-slider" type="range" min="1" max="100" value="50"/>

                <div className="toggler-preference">
                    <label htmlFor="toggler">Only show pets in this range</label>
                    <label className="switch">
                        <input className="toggler" type="checkbox" id="toggler"/>
                        <span className="slider round"></span>
                    </label>
                </div>    
            </div>
            
            <h3 className="sidebar-title">APPEARANCE</h3>
            <button className="preference-btn">Light Mode</button>
            <button className="preference-btn">DarkMode</button>

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
        </div>
        );
}