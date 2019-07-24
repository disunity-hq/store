import Vue from 'vue';

import OrgUsersTable from 'shared/vue/OrgUsersTable.vue';

export function InitPageScript() {
  new Vue({
    el: '#usersTable',
    template: '<OrgUsersTable/>',
    components: {
      OrgUsersTable
    }
  });
}
