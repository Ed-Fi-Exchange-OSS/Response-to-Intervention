import vue from 'vue'
import AlertDialog from './dialogs/AlertDialog'
import LoadingDialog from './dialogs/LoadingDialog'
import NavigationItemsCard from './NavigationItemsCard'
import OdsApiSettingsForm from './OdsApiSettingsForm'
import SharedIdleDialog from "@bit/edgraph.shared.idle-dialog"
import SharedLoader from "@bit/edgraph.shared.loading-component"
import SharedSideNav from '@bit/edgraph.shared.side-nav'
import SharedTopBar from "@bit/edgraph.shared.top-bar"
import StatusFooter from './StatusFooter'
import Toolbar from './widgets/AppToolbar'
import TopBar from './TopBar.vue'
import UserEmailMappingsItem from '@/views/UserEmailMappings/UserEmailMappingsItem'
import UserRoleMappingsForm from './UserRoleMappingsForm'

vue.component('AlertDialog', AlertDialog)
vue.component('LoadingDialog', LoadingDialog)
vue.component('NavigationItemsCard', NavigationItemsCard)
vue.component('OdsApiSettingsForm', OdsApiSettingsForm)
vue.component('SharedIdleDialog', SharedIdleDialog)
vue.component('SharedLoader', SharedLoader)
vue.component('SharedSideNav', SharedSideNav)
vue.component('SharedTopBar', SharedTopBar)
vue.component('StatusFooter', StatusFooter)
vue.component('Toolbar', Toolbar)
vue.component('TopBar', TopBar)
vue.component('UserEmailMappingsItem', UserEmailMappingsItem)
vue.component('UserRoleMappingsForm', UserRoleMappingsForm)
