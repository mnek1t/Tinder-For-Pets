import "../styles/profile.css"
import EmptyHeader from "./EmptyHeader";
import UserPreferences, { UserPreferencesProps } from "./UserPreferences"
import TabSelection from "./TabSelection";
import Navigation from "./Navigation";
interface SidebarProps extends Partial<UserPreferencesProps> { // partial means optional props
    isUserPreferences: boolean
} 

export default function Sidebar({
    isUserPreferences = true,
    distance,
    onToggleShowPets,
    handleLogout,
    handleDeleteAccount,
} : SidebarProps) {
   
    return(
        <div className="sidebar">
            <EmptyHeader/>
            <Navigation profileName="Profile"/>
            { isUserPreferences ? <UserPreferences 
                distance = {distance!}
                onToggleShowPets = {onToggleShowPets!}
                handleLogout = {handleLogout!}
                handleDeleteAccount = {handleDeleteAccount!}
            /> : <TabSelection/>}
            
        </div>
        );
}