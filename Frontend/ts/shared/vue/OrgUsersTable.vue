<template>
  <div>
    <table class="table no-top-border">
      <thead>
        <tr>
          <th scope="col">Username</th>
          <th scope="col">Role</th>
          <th scope="col"></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="membership in members" v-bind:key="membership.username">
          <td>{{ membership.userName }}</td>
          <td>
            <ejs-inplaceeditor
              v-if="canManageRoles && membership.role !== 'Owner'"
              v-bind:id="membership.userName + 'RoleEditor'"
              type="DropDownList"
              mode="Inline"
              :value="membership.role"
              :model="rolesModel"
              v-on:actionSuccess="changeRole($event, membership.userName)"
            ></ejs-inplaceeditor>
            <span v-else>{{membership.role}}</span>
          </td>
          <td class="smallCell" v-if="canManageMembers">
            <button
              id="btn-remove-member"
              v-if="canManageRoles && membership.role !== 'Owner'"
              type="button"
              class="btn btn-primary"
              v-on:click="removeMember(membership.userName)"
            >
              <i class="fas fa-minus" />
            </button>
          </td>
        </tr>
        <tr v-if="addingMember">
          <td>
            <div class="form-group">
              <ejs-autocomplete
                autofill="true"
                v-on:filtering="getUsernames($event)"
                v-on:change="userName=$event.value"
              ></ejs-autocomplete>
            </div>
          </td>
          <td>
            <div class="form-group">
              <select class="form-control" v-model="role">
                <option v-for="role in roles" v-bind:key="role">{{role}}</option>
              </select>
            </div>
          </td>
          <td class="smallCell">
            <button type="button" class="btn btn-primary" v-on:click="addMember">
              <i class="fas fa-save" />
            </button>
            <button type="button" class="btn btn-primary" v-on:click="addingMember = false">
              <i class="fas fa-trash" />
            </button>
          </td>
        </tr>
        <tr v-else-if="canManageRoles">
          <td></td>
          <td></td>
          <td key="member-add" id="ctrl-member-add">
            <button
              type="button"
              class="btn btn-primary"
              v-on:click="addingMember = !addingMember"
            >Add member</button>
          </td>
        </tr>
      </tbody>
    </table>
    <div id="orgMembersErrors"></div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { Component, Prop } from "vue-property-decorator";
import axios from "axios";
import { ActionEventArgs } from "@syncfusion/ej2-vue-inplace-editor";
import { DataManager, UrlAdaptor, Query } from "@syncfusion/ej2-data";
import "./syncfusion";
import ErrorReporter from "../ErrorReporter";
import { FilteringEventArgs } from "@syncfusion/ej2-dropdowns";

enum MemberRole {
  Member,
  Admin,
  Owner
}

interface IMembership {
  userName: string;
  role: string;
}

@Component
export default class OrgMembersTable extends Vue {
  @Prop({ type: Boolean, default: false }) readonly canManageRoles: boolean;
  @Prop({ type: Boolean, default: false }) readonly canManageMembers: boolean;
  @Prop({ type: String, required: true }) readonly orgSlug: string;

  members: IMembership[] = [];
  addingMember = false;

  userName: string = "";
  role: string = "Member";

  readonly roles = Object.keys(MemberRole).filter(k => isNaN(Number(k)));
  readonly rolesModel = {
    dataSource: this.roles
  };

  readonly baseUrl = `/api/v1/orgs/${this.orgSlug}/members`;
  readonly errorReporter = new ErrorReporter("#orgMembersErrors");

  public async mounted() {
    (DataManager as any).foo = "bar";

    try {
      const members = (await axios.get<IMembership[]>(this.baseUrl)).data;
      if (Array.isArray(members)) {
        this.members = members;
      }
    } catch (e) {
      console.error(e);
    }
  }

  public async addMember() {
    const membership: IMembership = {
      userName: this.userName,
      role: this.role
    };

    try {
      const response = await axios.post(this.baseUrl, membership);
      if (response.status === 204) {
        this.members.push(membership);
        this.addingMember = false;
        this.userName = "";
        this.role = "Member";
      } else {
        this.errorReporter.Empty();
        this.errorReporter.Set(response.data);
      }
    } catch (e) {
      console.error(e);
    }
  }

  public async removeMember(username: string) {
    try {
      const response = await axios.delete(`${this.baseUrl}/${username}`);

      if (response.status === 204) {
        this.members = this.members.filter(m => m.userName != username);
      } else {
        this.errorReporter.Empty();
        this.errorReporter.Set(response.data);
      }
    } catch (e) {
      console.error(e);
    }
  }

  public async changeRole($event: ActionEventArgs, userName: string) {
    const membership: IMembership = {
      userName,
      role: $event.value
    };
    try {
      const response = await axios.put(this.baseUrl, membership);

      if (response.status === 204) {
        this.members.find(m => m.userName === userName).role = membership.role;
      } else {
        this.errorReporter.Empty();
        this.errorReporter.Set(response.data);
      }
    } catch (e) {
      console.error(e);
    }
  }

  public async getUsernames($event: FilteringEventArgs) {
    $event.preventDefaultAction = true;
    // this.userName = $event.text;

    if ($event.text.length < 3) {
      $event.cancel = true;
      return;
    }

    const response = await axios.get(
      `/api/v1/users/autocomplete/?username=${$event.text}`
    );

    if (response.status === 200) {
      $event.updateData(response.data);
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@syncfusion/ej2-vue-inplace-editor/styles/bootstrap.scss";

.e-inplaceeditor {
  margin-left: 2em !important;
}

table {
  text-align: center;
  tr {
    td:first-child {
      width: 100%;
    }
    td:nth-child(2) {
      min-width: 200px;
    }
    td:last-child {
      min-width: 16em;
      #btn-remove-member {
        visibility: hidden;
      }
    }

    &:hover {
      td:last-child {
        #btn-remove-member {
          visibility: visible;
        }
      }
    }
  }
}
</style>
